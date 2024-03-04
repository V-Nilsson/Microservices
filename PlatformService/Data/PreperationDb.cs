using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data;
public static class PreperationDb
{
    public static void PreperationPopulation(IApplicationBuilder app, bool isProduction)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>(), isProduction);
    }

    private static void SeedData(ApplicationDbContext dbContext, bool isProduction)
    {
        if (isProduction)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }

        if (!dbContext.Platforms.Any())
        {
            Console.WriteLine("Seeding Data...");

            dbContext.Platforms.AddRange(
                new Models.Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost= "Free" },
                new Models.Platform() { Name = "SQL Server", Publisher = "Microsoft", Cost= "Free" }, 
                new Models.Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );

            dbContext.SaveChanges();
        }
    }
}