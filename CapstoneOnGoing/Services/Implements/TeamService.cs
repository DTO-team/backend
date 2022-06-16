using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
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

		public bool CreateTeam(CreateTeamRequest createTeamRequest,string userEmail, out CreatedTeamResponse createdTeamResponse)
		{
			//get Current Semester
			Semester currentSemester = _unitOfWork.Semester.Get(x => x.Status == (int)TeamStatus.Active).FirstOrDefault();
			//get User with role student
			User user = _unitOfWork.User.Get(x => (x.Email.Equals(userEmail) && x.RoleId == (int)RoleEnum.Student), null, "Student").FirstOrDefault();
			if (user != null)
			{
				// if student is in current Semester
				if (user.Student.SemesterId != null)
				{
					//check if student is in any team
					Student student = _unitOfWork.Student
						.Get(x => x.Id == user.Id, null, "TeamStudents").FirstOrDefault();
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
							Status = (int)TeamStatus.Active,
							TeamLeaderId = user.Id,
							JoinCode = GenerateUtil.GenerateJoinString(),
						};
						newTeam.TeamStudents.Add(new TeamStudent()
						{
							StudentId = user.Id,
							TeamId = newTeam.Id,
							Status = (int)TeamStudentStatus.Active,
						});
						_unitOfWork.Team.Insert(newTeam);
						bool isSuccessful = _unitOfWork.Save() > 0;
						if (isSuccessful)
						{
							createdTeamResponse = _mapper.Map<CreatedTeamResponse>(newTeam);
							createdTeamResponse.StudentCode = student.Code;
							_mapper.Map<User, CreatedTeamResponse>(user, createdTeamResponse);
						}
						//assign to null if create team failed
						else
						{
							createdTeamResponse = null;
						}
						return isSuccessful;
					}
				}
				else
				{
					throw new BadHttpRequestException($"Student {user.Email} is not in-progress in current semester");
				}
			}
			else
			{
				throw new BadHttpRequestException("Student does not exist");
			}
		}

		public bool DeleteTeam(Guid deleteTeamId,string userEmail)
		{
			Team deletedTeam = _unitOfWork.Team.Get(x => x.Id == deleteTeamId, null, "Semester").FirstOrDefault();
			//check if deleted team is existed in database
			if (deletedTeam != null)
			{
				//check if team is in current semester
				if (deletedTeam.Semester.Status != (int)SemesterStatus.Preparing)
				{
					throw new BadHttpRequestException("Team can not delete");
				}
				//check if is the team leader delete the team
				User teamLeader = _unitOfWork.User.Get(x => (x.Email == userEmail && x.StatusId == (int)UserStatus.Activated)).FirstOrDefault();
				if (teamLeader.Id == deletedTeam.TeamLeaderId)
				{
					deletedTeam.Status = (int)TeamStatus.Deleted;
					_unitOfWork.Team.Update(deletedTeam);
					return _unitOfWork.Save() > 0;
				}
				else
				{
					throw new BadHttpRequestException("Only the team leader can delete team");
				}

			}
			else
			{
				throw new BadHttpRequestException("Team is not existed");
			}
		}

		public IEnumerable<GetTeamResponse> GetAllTeams(string teamName = null, int page = 1, int limit = 10)
		{
			IEnumerable<Team> teamsResult = null;
			if (!string.IsNullOrEmpty(teamName) || !string.IsNullOrWhiteSpace(teamName))
			{
				teamsResult = _unitOfWork.Team.Get(team => team.Name == teamName,null,"TeamStudents",page,limit);
				foreach (Team team in teamsResult)
				{
					User teamLeader = _unitOfWork.User.Get(x => x.Id == team.TeamLeaderId,null, "Student,Role").FirstOrDefault();
					GetTeamResponse teamResponse = _mapper.Map<GetTeamResponse>(team);
					_mapper.Map<User, Leader>(teamLeader, teamResponse.LeaderShip);
					teamResponse.Amount = team.TeamStudents.Count;
					yield return teamResponse;
				}
			}
			else
			{
				teamsResult = _unitOfWork.Team.Get(null, null, "TeamStudents", page, limit);
				foreach (Team team in teamsResult)
				{
					User teamLeader = _unitOfWork.User.Get(x => x.Id == team.TeamLeaderId, null, "Student,Role").FirstOrDefault();
					teamLeader.Student.Semester = _unitOfWork.Semester.GetById(teamLeader.Student.SemesterId.Value);
					GetTeamResponse teamResponse = _mapper.Map<GetTeamResponse>(team);
					teamResponse.LeaderShip = new Leader();
					_mapper.Map<User, Leader>(teamLeader, teamResponse.LeaderShip);
					teamResponse.Amount = team.TeamStudents.Count;
					yield return teamResponse;
				}
			}
		}

		public bool JoinTeam(Guid id, string studentEmail)
		{
			throw new NotImplementedException();
		}
	}
}
