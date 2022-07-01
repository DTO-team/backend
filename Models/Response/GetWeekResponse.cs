using System;

namespace Models.Response
{
	public class GetWeekResponse
	{
		public Guid Id { get; set; }
		public int Number { get; set; }
		public Guid Semester { get; set; }
		public long FromDate { get; set; }
		public long ToDate { get; set; }
		public long Deadline { get; set; }
	}
}