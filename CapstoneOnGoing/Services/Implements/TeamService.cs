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

        public bool CreateTeam(CreateTeamRequest createTeamRequest, string userEmail, out CreatedTeamResponse createdTeamResponse)
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

        public bool DeleteTeam(Guid deleteTeamId, string userEmail)
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
                teamsResult = _unitOfWork.Team.Get(team => team.Name == teamName, null, "TeamStudents", page, limit);
                foreach (Team team in teamsResult)
                {
                    User teamLeader = _unitOfWork.User.Get(x => x.Id == team.TeamLeaderId, null, "Student,Role").FirstOrDefault();
                    GetTeamResponse teamResponse = _mapper.Map<GetTeamResponse>(team);
                    _mapper.Map<User, Member>(teamLeader, teamResponse.Leader);
                    Application teamApplication = _unitOfWork.Applications.Get(application => application.TeamId.Equals(team.Id) && application.StatusId.Equals((int)ApplicationStatus.Approved))
                        .FirstOrDefault();
                    if (teamApplication is not null)
                    {
                        teamResponse.IsApplicationApproved = true;
                    }
                    else
                    {
                        teamResponse.IsApplicationApproved = false;
                    }

                    Project teamProject =
                        _unitOfWork.Project.Get(project => project.TeamId.Equals(team.Id)).FirstOrDefault();
                    if (teamProject is not null)
                    {
                        teamResponse.ProjectId = teamProject.Id;
                    }
                    else
                    {
                        teamResponse.ProjectId = null;
                    }

                    teamResponse.TotalMember = team.TeamStudents.Count;
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
                    teamResponse.Leader = new Member();
                    _mapper.Map<User, Member>(teamLeader, teamResponse.Leader);
                    Application teamApplication = _unitOfWork.Applications.Get(application => application.TeamId.Equals(team.Id) && application.StatusId.Equals((int)ApplicationStatus.Approved))
                        .FirstOrDefault();
                    if (teamApplication is not null)
                    {
                        teamResponse.IsApplicationApproved = true;
                    }
                    else
                    {
                        teamResponse.IsApplicationApproved = false;
                    }

                    Project teamProject =
                        _unitOfWork.Project.Get(project => project.TeamId.Equals(team.Id)).FirstOrDefault();
                    if (teamProject is not null)
                    {
                        teamResponse.ProjectId = teamProject.Id;
                    }
                    teamResponse.TotalMember = team.TeamStudents.Count;
                    yield return teamResponse;
                }
            }
        }

        public bool JoinTeam(string studentEmail, string joinCode, out GetTeamDetailResponse getTeamDetailResponse)
        {
            //check if team is existed
            Team team = _unitOfWork.Team.Get(x => x.JoinCode.Equals(joinCode), null, "TeamStudents").FirstOrDefault();
            if (team != null)
            {
                //check if student is in progress of current semester
                Semester semester = _unitOfWork.Semester.Get(x => x.Status == (int)SemesterStatus.Preparing)
                    .FirstOrDefault();
                User student = _unitOfWork.User.Get(x => x.Email == studentEmail, null, "Student").FirstOrDefault();
                if (student != null && student.Student.SemesterId.Equals(semester?.Id))
                {
                    //check if student is any others team
                    bool isExistedInOthersTeam = _unitOfWork.TeamStudent.Get(x => x.StudentId == student.Id).Any();
                    if (isExistedInOthersTeam)
                    {
                        throw new BadHttpRequestException("Student is in other team");
                    }
                    else
                    {
                        team.TeamStudents.Add(new TeamStudent()
                        {
                            TeamId = team.Id,
                            StudentId = student.Id,
                            Status = (int)TeamStudentStatus.Active
                        });
                        _unitOfWork.Team.Update(team);

                        bool isSuccessfully = _unitOfWork.Save() > 0;
                        if (isSuccessfully)
                        {
                            getTeamDetailResponse = GetTeamDetail(team);
                            return isSuccessfully;
                        }
                        else
                        {
                            getTeamDetailResponse = null;
                            return isSuccessfully;
                        }
                    }
                }
                else
                {
                    throw new BadHttpRequestException("Student is not in-progress of current semester");
                }
            }
            else
            {
                throw new BadHttpRequestException("Team does not existed");
            }
        }

        public GetTeamDetailResponse GetTeamDetail(Guid teamId)
        {
            // Team team = _unitOfWork.Team.Get(x => x.Id == teamId, null, "TeamStudents,Project").FirstOrDefault();
            Team team = _unitOfWork.Team.GetTeamWithProject(teamId);
            if (team != null)
            {
                GetTeamDetailResponse teamDetailResponse = GetTeamDetail(team);
                return teamDetailResponse;
            }
            else
            {
                throw new BadHttpRequestException($"Team with {teamId} teamId is not existed");
            }
        }

        private GetTeamDetailResponse GetTeamDetail(Team team)
        {
            User teamLeader = _unitOfWork.User.Get(x => x.Id == team.TeamLeaderId, null, "Student,Role").FirstOrDefault();
            teamLeader.Student.Semester = _unitOfWork.Semester.GetById(teamLeader.Student.SemesterId.Value);
            GetTeamDetailResponse teamResponse = _mapper.Map<GetTeamDetailResponse>(team);

            Application teamApplication = _unitOfWork.Applications.Get(application => application.TeamId.Equals(team.Id) && application.StatusId.Equals((int)ApplicationStatus.Approved))
                .FirstOrDefault();
            if (teamApplication is not null)
            {
                teamResponse.IsApplicationApproved = true;
            }
            else
            {
                teamResponse.IsApplicationApproved = false;
            }

            Project teamProject =
                _unitOfWork.Project.Get(project => project.TeamId.Equals(team.Id)).FirstOrDefault();
            if (teamProject is not null)
            {
                teamResponse.ProjectId = teamProject.Id;
            }
            else
            {
                teamResponse.ProjectId = Guid.Empty;
            }

            teamResponse.Leader = new Member();
            IList<Member> members = new List<Member>();
            _mapper.Map<User, Member>(teamLeader, teamResponse.Leader);
            //Get student details in team
            Array.ForEach(team.TeamStudents.ToArray(), student =>
            {
                User studentInTeam = _unitOfWork.User.Get(x => x.Id == student.StudentId, null, "Student,Role").FirstOrDefault();
                studentInTeam.Student.Semester = _unitOfWork.Semester.GetById(studentInTeam.Student.SemesterId.Value);
                Member member = _mapper.Map<Member>(studentInTeam);
                members.Add(member);
            });
            // team.Project = _unitOfWork.Project.GetProjectWithLecturersByTeamId(team.Id);
			//get mentors for project
			teamResponse.Mentors = new List<GetLecturerResponse>();
			if (team.Project != null)
			{
                Array.ForEach(team.Project.Mentors.ToArray(), mentor =>
                {
	                User mentorDetail = _unitOfWork.User.Get(x => x.Id == mentor.LecturerId,null, "Role,Lecturer").FirstOrDefault();
	                mentorDetail.Lecturer.Department =
		                _unitOfWork.Department.GetById(mentorDetail.Lecturer.DepartmentId);
	                GetLecturerResponse mentorResponse = _mapper.Map<GetLecturerResponse>(mentorDetail);
                    teamResponse.Mentors.Add(mentorResponse);
                });
			}
            teamResponse.Members = members;
            teamResponse.TotalMember = members.Count();
            return teamResponse;
        }

        public bool IsTeamLeader(Guid userId)
        {
            Team team = _unitOfWork.Team.Get(x => x.TeamLeaderId == userId).FirstOrDefault();
            if (team is not null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Return team Id for controller to get team detail
        //and return response of team detail
        public Guid UpdateTeamMentor(UpdateMentorRequest updateMentorRequest)
        {
            Guid responseUpdatedTeamId = Guid.Empty;
            string operation = updateMentorRequest.Op.ToLower();
            Guid projectId = updateMentorRequest.ProjectId;

            //Check operation format
            if (operation.ToLower() == "add" || operation.ToLower() == "remove")
            {
                //Check project is existed or not
                Project project = _unitOfWork.Project.GetById(projectId);
                if (project is not null)
                {
                    //Check team in of project is existed or not
                    Guid teamId = project.TeamId;
                    Team team = _unitOfWork.Team.GetById(teamId);
                    if (team is not null)
                    {
                        if (team.SemesterId is not null)
                        {
                            //Check semester is ONGOING or not
                            Guid semesterId = (Guid)team.SemesterId;
                            Semester semesterOfTeam = _unitOfWork.Semester.GetById(semesterId);
                            int semesterStatus = semesterOfTeam.Status;
                            if (semesterStatus.Equals((int)SemesterStatus.Preparing))
                            {
                                switch (operation)
                                {
                                    //Add new mentor
                                    case "add":
                                        if (updateMentorRequest.NewLecturerId.Any() == true)
                                        {
                                            Array.ForEach(updateMentorRequest.NewLecturerId.ToArray(), newLecturerId =>
                                            {
                                                Mentor existMentor = _unitOfWork.Mentor.Get(mentor =>
                                                    (mentor.LecturerId.Equals(newLecturerId) &&
                                                     mentor.ProjectId.Equals(projectId))).FirstOrDefault();
                                                if (existMentor is null)
                                                {
                                                    Lecturer newLecturer = _unitOfWork.Lecturer.GetById(newLecturerId);
                                                    if (newLecturer is not null)
                                                    {

                                                        Mentor newMentor = new Mentor()
                                                        { ProjectId = projectId, LecturerId = newLecturerId };
                                                        _unitOfWork.Mentor.Insert(newMentor);

                                                    }
                                                    else
                                                    {
                                                        throw new BadHttpRequestException(
                                                            $"Lecturer with {newLecturerId} id is not existed to process!");
                                                    }
                                                }
                                                else
                                                {
                                                    throw new BadHttpRequestException(
                                                        "Lecturer is already mentor this team");
                                                }
                                            });
                                            _unitOfWork.Save();
                                            responseUpdatedTeamId = teamId;
                                        }
                                        else
                                        {
                                            throw new BadHttpRequestException("New lecturer id list is empty");
                                        }
                                        break;

                                    //Delete mentor
                                    case "remove":
                                        if (updateMentorRequest.MentorId.Any() == true)
                                        {
                                            IEnumerable<Mentor> teamMentors =
                                                _unitOfWork.Mentor.Get(x => x.ProjectId.Equals(projectId));
                                            if (!teamMentors.Count().Equals(1))
                                            {
                                                Array.ForEach(updateMentorRequest.MentorId.ToArray(), deleteMentorId =>
                                                {
                                                    Mentor mentor = _unitOfWork.Mentor
                                                        .Get(mentor => (mentor.ProjectId.Equals(projectId) && mentor.Id.Equals(deleteMentorId)))
                                                        .FirstOrDefault();
                                                    if (mentor is not null)
                                                    {
                                                        _unitOfWork.Mentor.DeleteById(deleteMentorId);
                                                        _unitOfWork.Save();
                                                        responseUpdatedTeamId = teamId;
                                                    }
                                                    else
                                                    {
                                                        throw new BadHttpRequestException(
                                                            $"Mentor with {deleteMentorId} id is not mentor this team");
                                                    }
                                                });
                                            }
                                            else
                                            {
                                                throw new BadHttpRequestException("At least 1 mentor for 1 team");
                                            }
                                        }
                                        else
                                        {
                                            throw new BadHttpRequestException("Mentor id list is empty");
                                        }

                                        break;
                                    default:
                                        throw new BadHttpRequestException("Wrong operation request!");
                                }
                            }
                            else
                            {
                                throw new BadHttpRequestException("Only update for a team in preparing semester!");
                            }
                        }
                        else
                        {
                            throw new BadHttpRequestException($"Semester with {team.SemesterId} id is not existed!");
                        }
                    }
                    else
                    {
                        throw new BadHttpRequestException("Team of project is not found!");
                    }
                }
                else
                {
                    throw new BadHttpRequestException($"Project with {projectId} id is not existed!");
                }
            }
            else
            {
                throw new BadHttpRequestException("Wrong operation request!");
            }

            return responseUpdatedTeamId;
        }
    }
}

