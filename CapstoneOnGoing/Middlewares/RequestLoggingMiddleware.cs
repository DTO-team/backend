using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CapstoneOnGoing.Logger;
using Microsoft.AspNetCore.Http;

namespace CapstoneOnGoing.Middlewares
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILoggerManager _logger;

		public RequestLoggingMiddleware(RequestDelegate next, ILoggerManager logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task Invoke(HttpContext context)
		{
			context.Request.EnableBuffering();
			byte[] buffer = new byte[Convert.ToInt32(context.Request.ContentType)];
			await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
			string requestBody = Encoding.UTF8.GetString(buffer);
			context.Request.Body.Seek(0, SeekOrigin.Begin);

			StringBuilder builder = new StringBuilder(Environment.NewLine);
			foreach (var header in context.Request.Headers)
			{
				builder.Append($"{header.Key} : {header.Value}");
				builder.Append("\n");
			}

			builder.AppendLine($"Request body: {requestBody}");
			_logger.LogInfo(builder.ToString());
			await _next(context);
		}
	}
}
