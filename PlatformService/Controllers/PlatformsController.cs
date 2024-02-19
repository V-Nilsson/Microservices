using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Contract;
using PlatformService.Data;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository platformRepository;
    private readonly IMapper mapper;
    private readonly ICommandDataClient commandDataClient;

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper, ICommandDataClient commandDataClient)
    {
        this.platformRepository = platformRepository;
        this.mapper = mapper;
        this.commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformResponse>> GetPlatforms()
    {
        var platforms = platformRepository.GetAllPlatforms();
        var dto = mapper.Map<IEnumerable<PlatformResponse>>(platforms);
        return Ok(dto);
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformResponse> GetPlatformById(int id)
    {
        var platform = platformRepository.GetPlatformById(id);
        if (platform == null) return NotFound();
        var dto = mapper.Map<PlatformResponse>(platform);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<PlatformResponse>> CreatePlatform(PlatformRequest request)
    {
        var platformModel = mapper.Map<Platform>(request);
        platformRepository.CreatePlatform(platformModel);
        platformRepository.SaveChanges();

        var response = mapper.Map<PlatformResponse>(platformModel);

        try
        {
            await commandDataClient.SendPlatformToCommand(response);
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync($"Could not send synchronously: {ex}");
        }

        return CreatedAtRoute(nameof(GetPlatformById), new { response.Id }, response);
    }
}
