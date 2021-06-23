using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleArchitecture.Infrastructure.Identity;
using SimpleArchitecture.Infrastructure.Persistence;

namespace WebUI.API.Tests
{
    public class TestWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(descriptor);

                //services.AddScoped<INotificationService, NotificationService>();

                services.AddDbContext<ApplicationDbContext>(x =>
                {
                    SqliteDbContextOptionsBuilderExtensions.UseSqlite(x, "Filename=SimpleArchitectureDb.sqlite");
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<ApplicationDbContext>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();

                try
                {
                    dbContext.Database.Migrate();

                    var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                    ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager).Wait();
                    ApplicationDbContextSeed.SeedSampleDataAsync(dbContext).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "DB seeding error: {Message}", ex.Message);
                }
            });
        }
    }
}