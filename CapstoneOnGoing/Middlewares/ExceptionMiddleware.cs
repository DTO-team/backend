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
						var exception = contextFeature.Error as BadHttpRequestException;
						if (exception != null)
						{
							logger.LogWarn($"Error: {contextFeature.Error}");
							context.Response.StatusCode = exception.StatusCode;
							await context.Response.WriteAsync(new ErrorDetails()
							{
								StatusCode = exception.StatusCode,
								Error = exception.Message,
								TimeStamp = DateTime.Now
							}.ToString());
						}
						else
						{
							logger.LogError($"Error: {contextFeature.Error}");
							await context.Response.WriteAsync(new ErrorDetails()
							{
								StatusCode = context.Response.StatusCode,
								Error = contextFeature.Error.Message,
								TimeStamp = DateTime.Now
							}.ToString());
						}
					}
				});
			});
		}
	}
}
