using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Application
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public Guid TopicId { get; set; }
        public int StatusId { get; set; }

        public virtual Team Team { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual Project Project { get; set; }
    }
}
