using System;
using System.Collections.Generic;
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
		[Required(ErrorMessage = "List Criteria must have value")]
		public IEnumerable<CriteriaRequest> Criterias { get; set; }
	}

	public class CriteriaRequest
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Evaluation { get; set; }
	}
}