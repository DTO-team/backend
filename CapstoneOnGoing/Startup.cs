using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Middlewares;
using NLog;
using Repository;
using Microsoft.EntityFrameworkCore;
using CapstoneOnGoing.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace CapstoneOnGoing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", true)
                          .AddEnvironmentVariables(prefix: "CAPSTONEONGOING_");
                          
	        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = builder.Build();
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            //services.AddCors(options => {
            //    options.AddPolicy(name: MyAllowSpecificOrigins, 
            //                        policy => {
            //                            policy.WithOrigins("dto.codes","localhost");
            //                        });
            //});
            services.AddCors();
            services.AddAutoMapper(typeof(Startup));
            var connectionString = $"Server={Configuration.GetValue<string>("DATABASE_HOST")},{Configuration.GetValue<string>("DATABASE_PORT")};User Id={Configuration.GetValue<string>("DATABASE_USERNAME")};" +
                $"Password={Configuration.GetValue<string>("DATABASE_PASSWORD")};Database={Configuration.GetValue<string>("DATABASE_NAME")};";
            services.AddDbContext<CAPSTONEONGOINGContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddRepository();
            services.AddAuthentication(options =>{
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = Configuration.GetValue<string>("JWT_ISSUER"),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWT_SECRET_KEY"))),
            };
			});
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CapstoneOnGoing", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CapstoneOnGoing v1"));
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CapstoneOnGoing v1"));
            
            app.ConfigureExceptionHandler(logger);
            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseRouting();

            app.UseCors(x => x
                        //.AllowAnyMethod()
                        //.AllowAnyHeader()
                        //.AllowAnyOrigin());
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());



            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
