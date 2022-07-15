using System;
using System.Collections.Generic;

namespace Models.Request
{
    public class UpdateCriteriaRequest
    {
        public string Name { get; set; }
        public string Evaluation { get; set; }
        public ICollection<UpdateCriteriaGradeRequest> Grades { get; set; }
        public ICollection<UpdateCriteriaQuestionRequest> Questions { get; set; }
    }
}
