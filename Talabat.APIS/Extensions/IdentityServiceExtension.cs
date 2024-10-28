using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIS.Extensions
{
	public static class IdentityServiceExtension
	{


		public static IServiceCollection AddIdentityService( this IServiceCollection Services , IConfiguration configuration)
		{

			Services.AddScoped<ITokenService, TokenService>();

			Services.AddIdentity<AppUser, IdentityRole>()
					.AddEntityFrameworkStores<AppIdentityDbcontext>();
			// UserManager , SignInManager,RoleManager

			Services.AddAuthentication(Options =>
			{
				Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}) .AddJwtBearer(Options =>
					 {
						 Options.TokenValidationParameters = new TokenValidationParameters()
						 {
							 ValidateIssuer = true,
							 ValidIssuer = configuration["JWT:ValidIssuer"],
							 ValidateAudience = true,
							 ValidAudience = configuration["JWT:ValidAudience"],
							 ValidateLifetime = true,
							 ValidateIssuerSigningKey = true,
							 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))

						 };
					 }

				);
			return Services;
		}
	}
}
