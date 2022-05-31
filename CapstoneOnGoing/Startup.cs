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

namespace CapstoneOnGoing
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", true)
                          .AddEnvironmentVariables(prefix: "CAPSTONEONGOING_");
                          
	        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddCors(options => {
                options.AddPolicy(name: MyAllowSpecificOrigins, 
                                    policy => {
                                        policy.WithOrigins("dto.codes","localhost");
                                    });
            });
            services.AddAutoMapper(typeof(Startup));
            var connectionString = $"Server={Configuration.GetValue<string>("SERVER")},{Configuration.GetValue<string>("PORT")};User Id={Configuration.GetValue<string>("USERID")};" +
                $"Password={Configuration.GetValue<string>("PASSWORD")};Database={Configuration.GetValue<string>("DATABASE")};";
            services.AddDbContext<CAPSTONEONGOINGContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddRepository();
            //Valid Access Token
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                            {
                                //get JsonWebKeySet from AWS
                                var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                                //seriablize the result 
                                var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                                return (IEnumerable<SecurityKey>)keys;
                            },
                            ValidIssuer = $"https://cognito-idp.{Configuration.GetValue<string>("REGION")}.amazonaws.com/{Configuration.GetValue<string>("POOLID")}",
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            ValidAudience = "{Cognito AppClientID}",
                            ValidateAudience = true,
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

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
