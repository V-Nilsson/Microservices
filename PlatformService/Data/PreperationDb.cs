namespace PlatformService.Data;
public static class PreperationDb
{
    public static void PreperationPopulation(IApplicationBuilder app)
    {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<ApplicationDbContext>());
        }
    }

    private static void SeedData(ApplicationDbContext dbContext)
    {
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
