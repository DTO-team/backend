using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class CouncilProject
    {
        public Guid Id { get; set; }
        public Guid CouncilId { get; set; }
        public Guid ProjectId { get; set; }

        public virtual Council Council { get; set; }
        public virtual Project Project { get; set; }
    }
}
