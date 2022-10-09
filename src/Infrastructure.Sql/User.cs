using Infrastructure.Sql;
using Microsoft.AspNetCore.Identity;

namespace MovieRamaWeb.Data
{
    public class User : IdentityUser<int>
    {
        public ICollection<MovieReactionEntity> Reactions { get; set; } = new List<MovieReactionEntity>();
    }
}
