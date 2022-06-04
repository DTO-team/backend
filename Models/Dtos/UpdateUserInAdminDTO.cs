using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class UpdateUserInAdminDTO
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public int StatusId { get; set; }
    }
}
