﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Student
    {
        public Student()
        {
            TeamStudents = new HashSet<TeamStudent>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public bool InProgress { get; set; }
        public Guid SemesterId { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<TeamStudent> TeamStudents { get; set; }
    }
}