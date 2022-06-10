using System;
using System.IO;
using System.Net;
using CapstoneOnGoing.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Models;

namespace CapstoneOnGoing.Middlewares
{
	public static class ExceptionMiddleware
	{
		public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
		{
			app.UseExceptionHandler(appError =>
			{
				appError.Run(async context =>
				{
					context.Response.ContentType = "application/json";

					IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
					if (contextFeature != null)
					{
						logger.LogError($"Error: {contextFeature.Error}");
						var exception = contextFeature.Error as BadHttpRequestException;
						if (exception != null)
						{
							await context.Response.WriteAsync(new ErrorDetails()
							{
								StatusCode = exception.StatusCode,
								Message = exception.Message,
								TimeStamp = DateTime.Now
							}.ToString());
						}
						else
						{
							await context.Response.WriteAsync(new ErrorDetails()
							{
								StatusCode = context.Response.StatusCode,
								Message = contextFeature.Error.Message,
								TimeStamp = DateTime.Now
							}.ToString());
						}
					}
				});
			});
		}
	}
}
