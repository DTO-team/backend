﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Response
{
	public class LoginUserCompanyResponse
	{
		public Guid Id { get; set; }

		public string AccessToken { get; set; }

		public string Email { get; set; }

		public string FullName { get; set; }

		public string Role { get; set; }

		public int StatusId { get; set; }

		public string AvatarUrl { get; set; }
	}
}
