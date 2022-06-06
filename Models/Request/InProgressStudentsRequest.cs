using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Request
{
	public class InProgressStudentsRequest
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string StudentCode { get; set; }
		[Required]
		public string FullName { get; set; }
	}
}
