using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public Guid CriteriaId { get; set; }
        public string SubCriteria { get; set; }
    }
}
