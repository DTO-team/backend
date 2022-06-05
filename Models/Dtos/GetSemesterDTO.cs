using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
	public class GetSemesterDTO
	{
		public Guid Id { get; set; }
		public int Year { get; set; }
		public string Season { get; set; }
		public int Status { get; set; }
	}
}
