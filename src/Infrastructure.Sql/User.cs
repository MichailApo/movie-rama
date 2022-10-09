using Infrastructure.Sql;
using Microsoft.AspNetCore.Identity;

namespace MovieRamaWeb.Data
{
    public class User : IdentityUser<int>
    {
        public IEnumerable<MovieReactionEntity> Reactions { get; set; } = Enumerable.Empty<MovieReactionEntity>();
    }
}
