using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CapstoneOnGoing.Utils
{
	public class JwtUtil
	{
		private JwtUtil(){

		}

		public static JwtSecurityToken ValidateToken(string accesstoken)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			tokenHandler.ValidateToken(accesstoken, new TokenValidationParameters
			{
				IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
				{
					string downloadLink = $"https://cognito-idp.{Startup.Configuration.GetValue<string>("COGNITO_REGION")}.amazonaws.com/{Startup.Configuration.GetValue<string>("COGNITO_POOLID")}/.well-known/jwks.json";
					//get JsonWebKeySet from AWS
					var json = new WebClient().DownloadString(downloadLink);
					//seriablize the result 
					var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
					return (IEnumerable<SecurityKey>)keys;
				},
				ValidIssuer = $"https://cognito-idp.{Startup.Configuration.GetValue<string>("COGNITO_REGION")}.amazonaws.com/{Startup.Configuration.GetValue<string>("COGNITO_POOLID")}",
				ValidateIssuerSigningKey = true,
				ValidateIssuer = true,
				ValidateLifetime = true,
				ValidAudience = Startup.Configuration.GetValue<string>("COGNITO_APPCLIENTID"),
				ValidateAudience = true,
			}, out SecurityToken validatedToken);
			var jwtToken = (JwtSecurityToken)validatedToken;
			return jwtToken != null ? jwtToken : null;
		}

		public static string GetEmailFromJwtToken(JwtSecurityToken jwtToken)
		{
			string email = string.Empty;
			if (jwtToken == null)
			{
				//Handle later
			}
			email = jwtToken.Claims.First(x => x.Type == "email").Value;
			return email;
		}

		public static string GenerateJwtToken(string email,string role){
			var jwtHanlder = new JwtSecurityTokenHandler();
			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.Configuration.GetValue<string>("JWT_SECRET_KEY")));
			var credentials = new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256Signature);
			var issuer = Startup.Configuration.GetValue<string>("JWT_ISSUER");
			var claims = new List<Claim>(){
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Sub,email),
				new Claim(ClaimTypes.Role,role)
			};
			var expires = DateTime.Now.AddMinutes(Startup.Configuration.GetValue<long>("JWT_TOKEN_EXPIRE_TIME_IN_MINUTES"));

			var token = new JwtSecurityToken(issuer,null, claims, notBefore: DateTime.Now, expires, credentials);
			
			return jwtHanlder.WriteToken(token);
		}
	}
}
