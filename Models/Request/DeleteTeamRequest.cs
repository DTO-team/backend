using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Request
{
	public class DeleteTeamRequest
	{
		[Required]
		public Guid TeamLeaderId { get; set; }
		[Required]
		public Guid TeamId { get; set; }
	}
}
