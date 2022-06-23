using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
    public class UpdateMentorRequest
    {
        [Required]
        public string Op { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public IEnumerable<Guid> MentorId { get; set; }
        [Required]
        public IEnumerable<Guid> NewLecturerId { get; set; }
    }
}
