using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Response
{
    public class GetTopicAllProjectResponse
    {
        public Guid TopicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<GetLecturerResponse> Lecturer { get; set; }
        public GetUserResponse Company { get; set; }
    }
}
