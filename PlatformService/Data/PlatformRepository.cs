using Microsoft.AspNetCore.Http.HttpResults;
using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepository : IPlatformRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PlatformRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform == null) throw new ArgumentNullException(nameof(platform));
        _dbContext.Platforms.Add(platform);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _dbContext.Platforms.ToList();
    }

    public Platform GetPlatformById(int id)
    {
        return _dbContext.Platforms.FirstOrDefault(p => p.Id == id); 
            //?? throw new Exception($"No platform with id {id} was found");
    }

    public bool SaveChanges()
    {
        return (_dbContext.SaveChanges() >= 0);
    }
}
