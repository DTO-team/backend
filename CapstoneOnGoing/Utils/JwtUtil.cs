using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;

namespace CapstoneOnGoing.Utils
{
	public class JwtUtil
	{
		private JwtUtil(){

		}

		public static JwtSecurityToken ValidateToken(string accesstoken)
		{
			string test = Startup.Configuration.GetValue<string>("REGION");

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
            return "";
		}
	}
}
