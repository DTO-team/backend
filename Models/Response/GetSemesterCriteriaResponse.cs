using System;
using System.Collections.Generic;

namespace Models.Response
{
	public class GetSemesterCriteriaResponse
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Evaluation { get; set; }
	}
}