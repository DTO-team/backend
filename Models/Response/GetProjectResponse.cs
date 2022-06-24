using System;
using System.Collections.Generic;

namespace Models.Response
{
    public class GetProjectResponse
    {
        public GetTopicsResponse TopicsResponse { get; set; }
        public GetTeamDetailResponse TeamDetailResponse { get; set; }
    }
}
