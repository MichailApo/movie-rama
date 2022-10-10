using Microsoft.AspNetCore.Identity;
using MovieRamaWeb.Application.Services;
using System.Security.Claims;

namespace MovieRamaWeb.Data.Services
{
    public class IdentityAuthService : IAuthService
    {

        public Domain.User? GetUser(ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(id, out var intId))
            {
                return null;
            }

            return new Domain.User(intId, claimsPrincipal.FindFirstValue(ClaimTypes.Name));

        }
    }
}
