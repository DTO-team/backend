﻿using System.Net;
using CapstoneOnGoing.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Models;

namespace CapstoneOnGoing.Extensions
{
	public static class ExceptionMiddlewareExtension
	{
		public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
		{
			app.UseExceptionHandler(appError =>
			{
				appError.Run(async context =>
				{
					context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
					context.Response.ContentType = "application/json";

					IExceptionHandlerFeature contextFeature = context.Features.Get<IExceptionHandlerFeature>();
					if (contextFeature != null)
					{
						logger.LogError($"Error: {contextFeature.Error}");
						await context.Response.WriteAsync(new ErrorDetails()
						{
							StatusCode = context.Response.StatusCode,
							Message = "Internal Server Error"
						}.ToString());
					}
				});
			});
		}
	}
}
