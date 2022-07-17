using System;
using System.Collections.Generic;
using System.Linq;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Models;
using Models.Request;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
	public class CouncilService : ICouncilService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILoggerManager _logger;

		public CouncilService(IUnitOfWork unitOfWork, ILoggerManager logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
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
	}
}