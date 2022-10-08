using MovieRamaWeb.Domain;
using System.Security.Claims;

namespace MovieRamaWeb.Application.Services
{
    public interface IAuthService
    {

        User GetUser(ClaimsPrincipal claimsPrincipal);

    }
}
