using System;
using Models.Response;

namespace Models.Dtos
{
    public class GetTeamDto
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public TeamMemberDTO Leader { get; set; }
        public int TotalMember { get; set; }

        public GetTeamDto()
        {
        }
    }

    public class TeamMemberDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Semester { get; set; }
        public string Status { get; set; }
        public string AvatarUrl { get; set; }
	}
}
