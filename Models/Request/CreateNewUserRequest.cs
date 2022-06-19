using System.ComponentModel.DataAnnotations;

namespace Models.Dtos
{
    public class CreateNewUserRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int StatusId { get; set; }
    }
}
