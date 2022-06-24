using System;
using System.Collections.Generic;
using Models.Response;

namespace Models.Dtos
{
    public class GetProjectDetailDTO
    {
        public GetTopicsDTO Topics { get; set; }
        // public IList<GetLecturerDTO> Mentors { get; set; }
        public GetTeamDetailResponse TeamDetailResponse { get; set; }

    }
}
