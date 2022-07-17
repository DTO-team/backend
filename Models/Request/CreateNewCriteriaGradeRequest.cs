using System;

namespace Models.Request
{
    public class CreateNewCriteriaGradeRequest
    {
        public string Level { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public string Description { get; set; }
    }
}
