using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class UpdateUserInAdminDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Role { get; set; }
        public int StatusId { get; set; }
    }
}
