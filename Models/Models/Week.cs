using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Week
    {
        public Week()
        {
            Reports = new HashSet<Report>();
        }

        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid SemesterId { get; set; }
        public long FromDate { get; set; }
        public long ToDate { get; set; }
        public long Deadline { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
