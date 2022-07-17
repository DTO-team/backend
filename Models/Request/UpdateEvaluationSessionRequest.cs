using System;

namespace Models.Request
{
	public class UpdateEvaluationSessionRequest
	{
		public Guid Id { get; set; }
		public int Status { get; set; }
		public CreateCouncilRequest CreateCouncilRequest { get; set; }
	}
}