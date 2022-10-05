using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieRamaWeb.Data
{
    public class MovieRamaDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MovieEntity> Movies { get; set; }

        public MovieRamaDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieEntity>().ToTable("Movies"); ;
            modelBuilder.Entity<MovieEntity>().HasKey(movie => movie.Id);
            modelBuilder.Entity<MovieEntity>().Property(movie => movie.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<MovieEntity>().Property(movie => movie.Description).IsRequired();
            modelBuilder.Entity<MovieEntity>().Property(movie => movie.CreatorId).HasColumnName("CreatedBy");
            modelBuilder.Entity<MovieEntity>().HasOne(movie => movie.Creator);
            modelBuilder.Entity<MovieEntity>().HasIndex(movie => movie.CreatorId);
            modelBuilder.Entity<MovieEntity>().HasIndex(movie => movie.CreatedAt);
        }
    }
}
