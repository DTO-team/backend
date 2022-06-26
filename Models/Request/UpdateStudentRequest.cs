using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Request
{
    public class UpdateStudentRequest
    {
        [Required]
        public Guid Id { get; set; }
        public string Email { get; set; }
    
        public string UserName { get; set; }
     
        public string FullName { get; set; }
     
        public string Code { get; set; }

        public string AvatarUrl { get; set; }

    }
}
