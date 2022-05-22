using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Council
    {
        public Guid Id { get; set; }
        public Guid CouncilId { get; set; }
        public Guid LecturerId { get; set; }

        public virtual Council Council { get; set; }
        public virtual Lecturer Lecturer { get; set; }
    }
}
