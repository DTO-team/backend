using System;

namespace Models.Response
{
    public class ResponseApplyTeamFields
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamSemesterSeason { get; set; }
        public StudentResponse LeaderStudent { get; set; }
    }

    public class ResponseTopicFields
    {
        public Guid TopicId { get; set; }
        public string TopicName { get; set; }
        public string Description { get; set; }
        // public string? CompanyName { get; set; }
    }
    public class GetApplicationResponse
    {
        public Guid ApplicationId { get; set; }
        public ResponseApplyTeamFields ApplyTeam { get; set; }
        public ResponseTopicFields Topic { get; set; }
        public string Status { get; set; }
    }
}
