﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
	public class ImportTopicsRequest
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public IEnumerable<string> LecturerEmail { get; set; }
		
		[EmailAddress]
		public string CompanyEmail { get; set; }
	}
}
