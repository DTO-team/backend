using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Response
{
	public class GetTopicsResponse
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public GetCompanyResponse CompanyDetail { get; set; }
		public IEnumerable<GetLecturerResponse> LecturersDetails { get; set; }
	}
}
