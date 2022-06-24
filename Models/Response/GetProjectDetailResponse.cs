using System;
using System.Collections.Generic;

namespace Models.Response
{
    public class GetProjectDetailResponse
    {
        public GetTopicsResponse TopicsResponse { get; set; }
        public GetTeamDetailResponse TeamDetailResponse { get; set; }
    }
}
