using System;
using Models.Response;

namespace Models.Dtos
{
    public class GetAllProjectsDetailDTO
    {
        public Guid ProjectId { get; set; }
        public GetTopicAllProjectDTO TopicsAllProjectDto { get; set; }
        public GetTeamDetailResponse TeamDetailResponse { get; set; }
    }
}
