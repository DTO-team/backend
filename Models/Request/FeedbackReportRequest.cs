using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
	public class FeedbackReportRequest
	{
		[Required(ErrorMessage = "Op must have value")]
		public string Op { get; set; }
		[Required(ErrorMessage = "Path must have value")]
		public string Path { get; set; }
		[Required(ErrorMessage = "Value is not empty")]
		public string Value { get; set; }
	}
}