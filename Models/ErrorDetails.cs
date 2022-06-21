﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
