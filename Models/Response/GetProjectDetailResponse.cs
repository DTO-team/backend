using System;
using System.Collections.Generic;

namespace Models.Response
{
    public class GetProjectDetailResponse
    {
        public Guid ProjectId { get; set; }
        public GetTopicsResponse TopicsResponse { get; set; }
        public GetTeamDetailResponse TeamDetailResponse { get; set; }
    }
}
