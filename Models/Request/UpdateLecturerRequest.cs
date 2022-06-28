using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Dtos
{
    public class UpdateLecturerRequest
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AvatarUrl { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
