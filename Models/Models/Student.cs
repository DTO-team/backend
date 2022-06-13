using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Student
    {
        public Student()
        {
            Reports = new HashSet<Report>();
            TeamStudents = new HashSet<TeamStudent>();
            Teams = new HashSet<Team>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid? SemesterId { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<TeamStudent> TeamStudents { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
