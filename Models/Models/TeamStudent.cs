using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class TeamStudent
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public Guid StudentId { get; set; }
        public int? Status { get; set; }

        public virtual Student Student { get; set; }
        public virtual Team Team { get; set; }
    }
}
