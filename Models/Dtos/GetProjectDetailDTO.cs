using System;
using System.Collections.Generic;
using Models.Response;

namespace Models.Dtos
{
    public class GetProjectDetailDTO
    {
        public Guid ProjectId { get; set; }
        public GetTopicsDTO Topics { get; set; }
        public GetTeamDetailResponse TeamDetailResponse { get; set; }

    }
}
