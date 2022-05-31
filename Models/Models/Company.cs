using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Company
    {
        public Company()
        {
            Topics = new HashSet<Topic>();
        }

        public Guid Id { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
    }
}
