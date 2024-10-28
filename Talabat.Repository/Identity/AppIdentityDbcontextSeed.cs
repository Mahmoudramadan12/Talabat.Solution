using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
	public static class AppIdentityDbcontextSeed
	{

		public static async Task SeedUserAsync(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var User = new AppUser()
				{
					DisplayName = "Aliaa Tarek",
					Email = "aliaatarek.route@gmail.com",
					UserName = "aliaatarek.route",
					PhoneNumber = "01234567891"
				};


				await userManager.CreateAsync(User , "Pa$$w0rd");
			}

		}

	}
}
