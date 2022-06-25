using System;

namespace Models.Dtos
{
	public class GetSemesterDTO
	{
		public Guid Id { get; set; }
		public int Year { get; set; }
		public string Season { get; set; }
		public int Status { get; set; }
	}
}
