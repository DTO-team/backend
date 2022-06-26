using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Response
{
    public class GetAllProjectDetailResponse
    {
        public GetTopicAllProjectResponse TopicsResponse { get; set; }
        public GetTeamDetailResponse TeamDetailResponse { get; set; }
    }
}
