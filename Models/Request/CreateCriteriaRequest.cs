using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
    public class CreateCriteriaRequest
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Evaluation { get; set; }
        public ICollection<CriterionGradeRequest> GradesRequest { get; set; }
        public ICollection<CriterionQuestionRequest> QuestionsRequest { get; set; }
    }
}
