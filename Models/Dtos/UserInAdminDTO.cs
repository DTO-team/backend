﻿using System;
using Models.Response;

namespace Models.Dtos
{
    public class UserInAdminDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string AvatarUrl { get; set; }
        public UserStatusResponse Status { get; set; }
    }
}
