using System;

namespace Models.Request
{
    public class CreateNewCriteriaQuestionRequest
    {
        public string Priority { get; set; }
        public string Description { get; set; }
        public string SubCriteria { get; set; }
    }
}
