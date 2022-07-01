using System;
using System.Collections.Generic;

namespace Models.Response
{
    public class GetTopicAllProjectResponse
    {
        public Guid TopicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public GetUserResponse CompanyDetail { get; set; }
        public IEnumerable<GetLecturerResponse> LecturersDetails { get; set; }
    }
}
