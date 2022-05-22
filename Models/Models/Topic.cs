using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Topic
    {
        public Topic()
        {
            Applications = new HashSet<Application>();
            TopicLecturers = new HashSet<TopicLecturer>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid SemesterId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<TopicLecturer> TopicLecturers { get; set; }
    }
}
