using System;

namespace Models.Response.GetAllUserResponse
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string AvatarUrl { get; set; }
    }
}
