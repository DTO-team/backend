using System;

namespace Models.Response
{
	public class GetGradeCopyResponse
	{
		public Guid Id { get; set; }
		public string Level { get; set; }
		public int MinPoint { get; set; }
		public int MaxPoint { get; set; }
		public string Description { get; set; }
	}
}