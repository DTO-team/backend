﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            CouncilLecturers = new HashSet<CouncilLecturer>();
            Feedbacks = new HashSet<Feedback>();
            Mentors = new HashSet<Mentor>();
            TopicLecturers = new HashSet<TopicLecturer>();
        }

        public Guid Id { get; set; }
        public Guid DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual User IdNavigation { get; set; }
        public virtual ICollection<CouncilLecturer> CouncilLecturers { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Mentor> Mentors { get; set; }
        public virtual ICollection<TopicLecturer> TopicLecturers { get; set; }
    }
}
