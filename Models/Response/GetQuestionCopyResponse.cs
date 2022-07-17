using System;

namespace Models.Response
{
	public class GetQuestionCopyResponse
	{
		public Guid Id { get; set; }
		public string Description { get; set; }
		public string Priority { get; set; }
	}
}