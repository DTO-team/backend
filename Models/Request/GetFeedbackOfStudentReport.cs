using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Request
{
    public class GetFeedbackOfStudentReport: GetFeedbackRequest
    {
        [Required]
        public Guid StudentId { get; set; }
    }
}
