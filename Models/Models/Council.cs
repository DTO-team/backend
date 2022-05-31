using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Council
    {
        public Council()
        {
            CouncilLecturers = new HashSet<CouncilLecturer>();
            CouncilProjects = new HashSet<CouncilProject>();
            Reviews = new HashSet<Review>();
        }

        public Guid Id { get; set; }
        public Guid EvaluationSessionId { get; set; }

        public virtual EvaluationSession EvaluationSession { get; set; }
        public virtual ICollection<CouncilLecturer> CouncilLecturers { get; set; }
        public virtual ICollection<CouncilProject> CouncilProjects { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
