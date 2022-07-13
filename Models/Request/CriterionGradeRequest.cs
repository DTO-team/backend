using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
    public class CriterionGradeRequest
    {
        [Required]
        public string Level { get; set; }
        [Required]
        public int MinPoint { get; set; }
        [Required]
        public int MaxPoint { get; set; }
        public string Description { get; set; }
    }
}
