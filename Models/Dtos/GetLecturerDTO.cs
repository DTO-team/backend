using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
	public class GetLecturerDTO
	{
		public Guid Id { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string FullName { get; set; }
		public string Role { get; set; }
		public int StatusId { get; set; }
		public string AvatarUrl { get; set; }
		public GetDepartmentDTO Department { get; set; }
    }
}
