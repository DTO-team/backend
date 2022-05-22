using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Mentor
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public Guid LecturerId { get; set; }

        public virtual Lecturer Lecturer { get; set; }
        public virtual Team Team { get; set; }
    }
}
