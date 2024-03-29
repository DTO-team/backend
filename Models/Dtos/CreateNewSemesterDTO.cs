﻿

using System.ComponentModel.DataAnnotations;

namespace Models.Dtos
{
	public class CreateNewSemesterDTO
	{
		[Required]
		public int Year { get; set; }
		[Required]
		public string Season { get; set; }
		[Required]
		public long StartDate { get; set; }
		[Required]
		public long EndDate { get; set; }
	}
}
