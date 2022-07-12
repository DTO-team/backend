using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class GradeDTO
    {
        public Guid Id { get; set; }
        public Guid CriteriaId { get; set; }
        public string Level { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public string Description { get; set; }
    }
}
