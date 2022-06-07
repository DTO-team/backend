using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Response
{
	public class GenericResponse
	{
		public int HttpStatus { get; set; }
		public string Message { get; set; }
		public DateTime TimeStamp { get; set; }
	}
}
