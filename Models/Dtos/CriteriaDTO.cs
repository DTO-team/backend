using System;
using System.Collections.Generic;

namespace Models.Dtos
{
    public class CriteriaDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Evaluation { get; set; }

        public ICollection<GradeDTO> Grades { get; set; }
        public ICollection<QuestionDTO> Questions { get; set; }
    }
}
