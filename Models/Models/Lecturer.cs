using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            CouncilLecturers = new HashSet<Council>();
            Mentors = new HashSet<Mentor>();
            Reviews = new HashSet<Review>();
            TopicLecturers = new HashSet<TopicLecturer>();
        }

        public Guid Id { get; set; }
        public string Department { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual ICollection<Council> CouncilLecturers { get; set; }
        public virtual ICollection<Mentor> Mentors { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<TopicLecturer> TopicLecturers { get; set; }
    }
}
