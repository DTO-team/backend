using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Dtos
{
    public class UpdateLecturerRequest
    {
        [Required]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }

    }
}
