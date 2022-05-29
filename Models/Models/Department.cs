using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Department
    {
        public Department()
        {
            Lecturers = new HashSet<Lecturer>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Lecturer> Lecturers { get; set; }
    }
}
