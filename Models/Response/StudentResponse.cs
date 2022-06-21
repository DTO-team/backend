﻿using System;

namespace Models.Response
{
    public class StudentResponse
    {
        public Guid Id { get; set; }
        public string TeamId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Semester { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}
