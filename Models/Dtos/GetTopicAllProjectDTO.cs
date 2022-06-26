using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class GetTopicAllProjectDTO
    {
        public Guid TopicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Guid> LecturerIds { get; set; }
        public Guid? CompanyId { get; set; }
    }
}
