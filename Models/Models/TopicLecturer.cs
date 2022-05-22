using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class TopicLecturer
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public Guid LecturerId { get; set; }

        public virtual Lecturer Lecturer { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
