using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public int StatusId { get; set; }

        public virtual Role Role { get; set; }
        public virtual UserStatus Status { get; set; }
        public virtual Company Company { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public virtual Student Student { get; set; }
    }
}
