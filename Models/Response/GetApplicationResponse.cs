namespace Models.Response
{
    public class ResponseApplyTeamFields
    {
        public string TeamName { get; set; }
        public string LeaderName { get; set; }
        public string TeamSemesterSeason { get; set; }
    }

    public class ResponseTopicFields
    {
        public string TopicName { get; set; }
        public string Description { get; set; }
        // public string? CompanyName { get; set; }
    }
    public class GetApplicationResponse
    {
        public ResponseApplyTeamFields ApplyTeam { get; set; }
        public ResponseTopicFields Topic { get; set; }
        public string Status { get; set; }
    }
}
