using System;

namespace Models.Response.GetAllUserResponse
{
    public class StudentUserResponse : UserResponse
    {
        public string studentCode { get; set; }
        public string Semester { get; set; }
    }
}
