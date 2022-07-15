using System;

namespace Models.Request
{
    public class UpdateCriteriaQuestionRequest
    {
        public Guid Id { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string SubCriteria { get; set; }
    }
}
