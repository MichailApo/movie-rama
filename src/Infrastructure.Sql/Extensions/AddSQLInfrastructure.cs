using Microsoft.Extensions.DependencyInjection;
using MovieRamaWeb.Application.Services;
using MovieRamaWeb.Data.Repositories;
using MovieRamaWeb.Data.Services;
using MovieRamaWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Application.Services;
using Infrastructure.Sql.Repositories;

namespace Infrastructure.Sql.Extensions
{
    public static class SQLInfrastructureExtensions
    {
        public static IServiceCollection AddSQLInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, IdentityAuthService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IReactionRepository, ReactionRepository>();
            var connectionString = configuration.GetConnectionString("MovieRamaDbContextConnection") ?? throw new InvalidOperationException("Connection string 'MovieRamaDbContextConnection' not found.");
            services.AddDbContext<MovieRamaDbContext>(options => { 
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();
            });

            return services;
        }

        public static void EnsureDatabaseCreated(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var tries = 5;
                var retryAfterSeconds = 5;
                var currentTry = 0;
                while (currentTry < tries)
                {
                    try
                    {
                        var context = serviceProvider.GetRequiredService<MovieRamaDbContext>();
                        if (context.Database.IsRelational())
                        {
                            context.Database.Migrate();
                        }
                        return;
                    }
                    catch
                    {
                        currentTry++;
                        Thread.Sleep(TimeSpan.FromSeconds(retryAfterSeconds));
                    }
                }
            }
        }

    }
}
