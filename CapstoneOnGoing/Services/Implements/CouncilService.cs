using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
	public class CouncilService : ICouncilService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILoggerManager _logger;
		private readonly ILecturerService _lecturerService;
		private readonly IProjectService _projectService;
		private readonly IMapper _mapper;

		public CouncilService(IUnitOfWork unitOfWork, ILoggerManager logger, ILecturerService lecturerService, IProjectService projectService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_lecturerService = lecturerService;
			_projectService = projectService;
			_mapper = mapper;
		}

		public void CreateCouncil(CreateCouncilRequest createCouncilRequest)
		{
			if (createCouncilRequest == null)
			{
				throw new BadHttpRequestException("Create Council Request have no data");
			}
			EvaluationSession evaluationSession = _unitOfWork.EvaluationSession.GetById(createCouncilRequest.EvaluationSessionId);
			ICollection<Project> projects = new List<Project>();

			Array.ForEach(createCouncilRequest.ProjectId.ToArray(), projectid =>
			{
				Project project = _unitOfWork.Project.Get(x => x.Id == projectid, null, "Mentors")
					.FirstOrDefault();
				project.Application =
					_unitOfWork.Applications.GetApplicationWithTeamTopicProject(project.ApplicationId);
				projects.Add(project);
			});
			IEnumerable<User> lecturers = _unitOfWork.User.GetLecturersByIds(createCouncilRequest.LecturerId.ToArray());
			if (evaluationSession == null || !projects.Any() || !lecturers.Any())
			{
				throw new BadHttpRequestException("Create Council request has invalid data");
			}
			//check if lecturers in councils is mentor of project
			bool isMentorOfProject = false;
			Array.ForEach(lecturers.ToArray(), lecturer =>
			{
				Array.ForEach(projects.ToArray(), project =>
				{
					isMentorOfProject =  project.Mentors.Select(x => x.LecturerId).Contains(lecturer.Id);
					if (isMentorOfProject)
					{
						throw new BadHttpRequestException(
							$"Lecturer {lecturer.FullName} is mentor of the {project.Application.Topic.Name} project");
					}
				});
			});
			//create new council
			Council newCouncil = new Council()
			{
				Id = Guid.NewGuid()
			};
			ICollection<CouncilLecturer> councilLecturers = new List<CouncilLecturer>();
			ICollection<CouncilProject> councilProjects = new List<CouncilProject>();
			Array.ForEach(lecturers.ToArray(), lecturer =>
			{
				councilLecturers.Add(new CouncilLecturer()
				{
					Id = Guid.NewGuid(),
					CouncilId = newCouncil.Id,
					LecturerId = lecturer.Id,
				});
			});
			Array.ForEach(projects.ToArray(), project =>
			{
				councilProjects.Add(new CouncilProject()
				{
					Id = Guid.NewGuid(),
					CouncilId = newCouncil.Id,
					ProjectId = project.Id
				});
			});
			newCouncil.EvaluationSessionId = evaluationSession.Id;
			newCouncil.CouncilProjects = councilProjects;
			newCouncil.CouncilLecturers = councilLecturers;
			_unitOfWork.Councils.Insert(newCouncil);
		}

        public IEnumerable<GetCouncilResponse> GetAllCouncils(GetSemesterDTO semester)
        {
	        IEnumerable<Council> councils = _unitOfWork.Councils.Get();
			List<GetCouncilResponse> councilResponsesList = new List<GetCouncilResponse>();
			Array.ForEach(councils.ToArray(), council =>
			{
				GetCouncilResponse councilResponse = GetCouncilById(council.Id, semester);
				councilResponsesList.Add(councilResponse);
			});
	        return councilResponsesList;
        }

        public GetCouncilResponse GetCouncilById(Guid councilId, GetSemesterDTO semester)
        {
	        Council council = _unitOfWork.Councils.Get(x => x.Id == councilId,null, "CouncilLecturers,CouncilProjects,EvaluationSession").FirstOrDefault();
	        if (council == null) throw new BadHttpRequestException("No council is found");
			EvaluationSession evaluationSession = _unitOfWork.EvaluationSession
				.Get(x => x.Id == council.EvaluationSessionId && x.SemesterId == semester.Id, null, "Semester,EvaluationSessionCriteria,Councils").FirstOrDefault();
			if (evaluationSession == null)
			{
				throw new BadHttpRequestException("No evaluation session found");
			}

			GetEvaluationSessionResponse evaluationSessionResponse =
				_mapper.Map<GetEvaluationSessionResponse>(evaluationSession);
			if (evaluationSession.EvaluationSessionCriteria.Any())
			{
				evaluationSessionResponse.SemesterCriterias = new List<GetSemesterCriteriaResponse>();
				Array.ForEach(evaluationSession.EvaluationSessionCriteria.ToArray(), evaluationSessionCriteria =>
				{
					IEnumerable<SemesterCriterion> semesterCriterion =
						_unitOfWork.SememesterCriterion.Get(x => x.Id == evaluationSessionCriteria.CriteriaId);
					IEnumerable<GetSemesterCriteriaResponse> semesterCriteriaResponses =
						_mapper.Map<IEnumerable<GetSemesterCriteriaResponse>>(semesterCriterion);
					evaluationSessionResponse.SemesterCriterias.AddRange(semesterCriteriaResponses);
				});
			}
			List<GetLecturerResponse> lecturerResponses = new List<GetLecturerResponse>();
	        List<GetProjectDetailDTO> projectDetailDtos = new List<GetProjectDetailDTO>();
			Array.ForEach(council.CouncilLecturers.ToArray(), councilLecturer =>
			{
				GetLecturerResponse lecturerResponse =
					_mapper.Map<GetLecturerResponse>(_lecturerService.GetLecturerById(councilLecturer.LecturerId));
				lecturerResponses.Add(lecturerResponse);
			});
			Array.ForEach(council.CouncilProjects.ToArray(), councilProject =>
			{
				GetProjectDetailDTO projectDetailDTO = _projectService.GetProjectDetailById(councilProject.ProjectId,semester);
				projectDetailDtos.Add(projectDetailDTO);
			});
			GetCouncilResponse councilResponse = _mapper.Map<GetCouncilResponse>(council);
			councilResponse.Lecturers = lecturerResponses;
			councilResponse.EvaluationSession = evaluationSessionResponse;
			councilResponse.Projects = projectDetailDtos;
			return councilResponse;
        }

        public bool UpdateCouncil(Guid id, UpdateCouncilRequest updateCouncilRequest)
        {
	        if (updateCouncilRequest == null)
	        {
		        throw new BadHttpRequestException("In valid value");
	        }

	        if (!updateCouncilRequest.LecturerIds.Any() && !updateCouncilRequest.ProjectIds.Any())
	        {
		        throw new BadHttpRequestException("No value for update");
	        }

	        IEnumerable<CouncilLecturer> councilLecturers = _unitOfWork.CouncilLecturer.Get(x => x.CouncilId == id);
	        IEnumerable<CouncilProject> councilProjects = _unitOfWork.CouncilProject.Get(x => x.CouncilId == id);

	        return true;
        }
	}
} 