using Infrastructure.Sql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MovieRamaWeb.Data
{
    public class MovieRamaDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<MovieEntity> Movies { get; set; }
        public virtual DbSet<MovieReactionEntity> MovieReactions { get; set; }

        public MovieRamaDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieEntity>().ToTable("Movies"); ;
            modelBuilder.Entity<MovieEntity>().HasKey(movie => movie.Id);
            modelBuilder.Entity<MovieEntity>().Property(movie => movie.Title);
            modelBuilder.Entity<MovieEntity>().Property(movie => movie.Description);
            modelBuilder.Entity<MovieEntity>().Property(movie => movie.CreatorId).HasColumnName("CreatedBy");
            modelBuilder.Entity<MovieEntity>().HasOne(movie => movie.Creator);
            modelBuilder.Entity<MovieEntity>().HasIndex(movie => movie.CreatorId);
            modelBuilder.Entity<MovieEntity>().HasIndex(movie => movie.CreatedAt);
            modelBuilder.Entity<MovieEntity>()
                .HasMany(movie => movie.Reactions)
                .WithOne(movie => movie.Movie)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MovieReactionEntity>().ToTable("Reaction");
            modelBuilder.Entity<MovieReactionEntity>().HasKey(reaction => new { reaction.MovieId, reaction.UserId });
            modelBuilder.Entity<MovieReactionEntity>().Property(reaction => reaction.UserId);
            modelBuilder.Entity<MovieReactionEntity>().Property(reaction => reaction.MovieId);
            modelBuilder.Entity<MovieReactionEntity>().Property(reaction => reaction.Active).HasDefaultValue(true);
            modelBuilder.Entity<MovieReactionEntity>().HasOne(reaction => reaction.Movie);
            modelBuilder.Entity<MovieReactionEntity>().HasOne(reaction => reaction.User);

            modelBuilder.Entity<User>()
                .HasMany(user => user.Reactions)
                .WithOne(user => user.User)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
