using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Response
{
	public class LoginUserLecturerResponse : LoginUserResponse
	{
		public GetDepartmentResponse Department { get; set; }
	}
}
