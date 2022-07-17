using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
	public class CreateCouncilRequest
	{
		[Required(ErrorMessage = "Evaluation Session Id is required")]
		public Guid EvaluationSessionId { get; set; }
		[Required(ErrorMessage = "Project Id is required")]
		public IEnumerable<Guid> ProjectId { get; set; }
		[Required(ErrorMessage = "Lecturer Id is required")]
		public IEnumerable<Guid> LecturerId { get; set; }
	}
}