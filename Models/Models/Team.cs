using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Team
    {
        public Team()
        {
            Applications = new HashSet<Application>();
            TeamStudents = new HashSet<TeamStudent>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<TeamStudent> TeamStudents { get; set; }
    }
}
