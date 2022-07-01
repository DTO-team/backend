﻿using System;

namespace Models.Response
{
	public class GetCompanyResponse
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
