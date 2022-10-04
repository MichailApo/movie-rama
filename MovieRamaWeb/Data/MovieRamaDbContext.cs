using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieRamaWeb.Data
{
    public class MovieRamaDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<User> Users { get; set; }

        public MovieRamaDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
