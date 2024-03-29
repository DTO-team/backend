﻿using System;

namespace Models.Dtos
{
	public class GetCompanyDTO
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string FullName { get; set; }
		public string Role { get; set; }
		public int StatusId { get; set; }
		public string AvatarUrl { get; set; }
	}
}
