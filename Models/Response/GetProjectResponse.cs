using System;

namespace Models.Response
{
    public class GetProjectResponse
    {
        public GetMentorDetail MentorDetail { get; set; }
        public GetTeamDetailResponse TeamDetail { get; set; }
        public GetTopicsResponse TopicDetail { get; set; }
    }
}
