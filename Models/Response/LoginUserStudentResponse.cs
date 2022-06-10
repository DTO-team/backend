using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Response
{
	public class LoginUserStudentResponse : LoginUserResponse
	{
		public string StudentCode { get; set; }

		public string Semester  { get; set; }

	}
}
