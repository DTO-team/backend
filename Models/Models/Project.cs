using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Project
    {
        public Project()
        {
            CouncilProjects = new HashSet<CouncilProject>();
        }

        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TeamId { get; set; }

        public virtual Application Application { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<CouncilProject> CouncilProjects { get; set; }
    }
}
