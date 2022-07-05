using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
    public class GetFeedbackRequest
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public Guid WeekId { get; set; }
    }
}
