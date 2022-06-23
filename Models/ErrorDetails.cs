using System;
using System.Text.Json;

namespace Models
{
	public class ErrorDetails
	{
		public int StatusCode { get; set; }
		public string Error { get; set; }
		public DateTime TimeStamp { get; set; }
		public override string ToString()
		{
			return JsonSerializer.Serialize(this);
		}
	}
}
