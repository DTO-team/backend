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
		public string LecturerEmail { get; set; }

		public string CompanyName { get; set; }
	}
}
