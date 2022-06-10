using System;

namespace Models.Response
{
	public class LoginUserResponse
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
