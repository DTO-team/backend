using System;

namespace Models.Response
{
    public class GetAllProjectDetailResponse
    {
        public Guid ProjectId { get; set; }
        public GetTopicAllProjectResponse TopicsResponse { get; set; }
        public GetTeamDetailResponse TeamDetailResponse { get; set; }
    }
}
