using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entites.Identity;

namespace Talabat.APIS.Extensions
{
	public static class UserManagerExtension
	{
		public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager , ClaimsPrincipal User)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await userManager.Users.Include(a=>a.Address).FirstOrDefaultAsync(u=>u.Email == email);

			return user;


		}



	}
}
