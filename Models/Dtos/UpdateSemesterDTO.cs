using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
	public class UpdateSemesterDTO
	{
		[Required]
		public Guid Id { get; set; }
		[Required]
		public int Status { get; set; }

	}
}
