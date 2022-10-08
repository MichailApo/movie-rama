using MovieRamaWeb.Domain;
using System.Security.Claims;

namespace MovieRamaWeb.Services
{
    public interface IAuthService
    {

        User GetUser(ClaimsPrincipal claimsPrincipal);

    }
}
