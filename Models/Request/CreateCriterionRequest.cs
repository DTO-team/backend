using System;
using System.Collections.Generic;

namespace Models.Request
{
    public class CreateCriterionRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Evaluation { get; set; }

        public ICollection<CriterionGradeRequest> GradesRequest { get; set; }
        public ICollection<CriterionQuestionRequest> QuestionsRequest { get; set; }
    }
}
