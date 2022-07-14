using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
	public class CreateEvaluationSessionRequest
	{
		[Required(ErrorMessage = "Round must have value")]
		public int Round { get; set; }
		[Required(ErrorMessage = "Is Final is required")]
		public bool IsFinal { get; set; }
		[Required(ErrorMessage = "Status is required")]
		public int Status { get; set; }
		[Required(ErrorMessage = "Deadline must have value")]
		public long Deadline { get; set; }
	}
}