using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class ApplicationStatus
    {
        public ApplicationStatus()
        {
            Applications = new HashSet<Application>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}
