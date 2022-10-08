using Microsoft.AspNetCore.Identity;
using MovieRamaWeb.Services;
using System.Security.Claims;

namespace MovieRamaWeb.Data.Services
{
    public class IdentityAuthService : IAuthService
    {

        public Domain.User GetUser(ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(id, out var intId))
            {
                throw new Exception();
            }

            return new Domain.User(intId, claimsPrincipal.FindFirstValue(ClaimTypes.Name));

        }
    }
}
