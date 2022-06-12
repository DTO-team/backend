using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using CapstoneOnGoing.Utils;
using Microsoft.AspNetCore.Http;
using Models.Models;
using Models.Request;
using Models.Response;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
	public class TeamService : ITeamService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public TeamService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public bool CreateTeam(CreateTeamRequest createTeamRequest, out CreatedTeamResponse createdTeamResponse)
		{
			//get Current Semester
			Semester currentSemester = _unitOfWork.Semester.Get(x => x.Status == 1).FirstOrDefault();
			//get User with role student
			User user = _unitOfWork.User.Get(x => (x.Id == createTeamRequest.StudentId && x.RoleId == 3), null, "Student").FirstOrDefault();
			if (user != null)
			{
				// if student is in current Semester
				if (user.Student.SemesterId != null)
				{
					//check if student is in any team
					Student student = _unitOfWork.Student
						.Get(x => x.Id == createTeamRequest.StudentId, null, "TeamStudents").FirstOrDefault();
					if (student.TeamStudents.Any())
					{
						throw new BadHttpRequestException("Student is in another team");
					}
					else
					{
						Team newTeam = new Team()
						{
							Name = createTeamRequest.TeamName,
							SemesterId = currentSemester.Id,
							Status = 1,
							TeamLeaderId = user.Id,
							JoinCode = GenerateUtil.GenerateJoinString(),
						};
						newTeam.TeamStudents.Add(new TeamStudent()
						{
							StudentId = user.Id,
							TeamId = newTeam.Id,
							Status = 1,
						});
						_unitOfWork.Team.Insert(newTeam);
						bool isSuccessful = _unitOfWork.Save() > 0;
						createdTeamResponse = _mapper.Map<CreatedTeamResponse>(newTeam);
						_mapper.Map<User, CreatedTeamResponse>(user, createdTeamResponse);
						return isSuccessful;
					}
				}
				else
				{
					throw new BadHttpRequestException("Student is not in current semester");
				}
			}
			else
			{
				throw new BadHttpRequestException("Student does not exist");
			}
		}

		public bool DeleteTeam()
		{

		}
	}
}
