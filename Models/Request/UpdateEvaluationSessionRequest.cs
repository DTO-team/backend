using System;

namespace Models.Request
{
	public class UpdateEvaluationSessionRequest
	{
		public int Status { get; set; }
		public CreateCouncilRequest CreateCouncilRequest { get; set; }
	}
}