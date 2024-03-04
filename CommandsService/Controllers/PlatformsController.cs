using AutoMapper;
using CommandsService.Contract;
using CommandsService.Data;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;
[Route("api/commandsservice/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepository repository;
    private readonly IMapper mapper;

    public PlatformsController(ICommandRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command Service");
        return Ok(0);
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformResponse>> GetAllPlatforms()
    {
        var platforms = repository.GetAllPlatforms();
        var dto = mapper.Map<IEnumerable<PlatformResponse>>(platforms);
        return Ok(dto);
    }
}