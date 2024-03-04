using AutoMapper;
using CommandsService.Contract;
using CommandsService.Data;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;
[Route("api/commandsservice/platforms/{platformsId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository repository;
    private readonly IMapper mapper;

    public CommandsController(ICommandRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandResponse>> GetCommandsForPlatform(int platformId)
    {
        if (!repository.PlatformExists(platformId)) return NotFound();

        var commands = repository.GetCommandsForPlatform(platformId);
        var dto = mapper.Map<IEnumerable<CommandResponse>>(commands);

        return Ok(dto);
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandResponse> GetCommandForPlatform(int platformId, int commandId) 
    {
        if (!repository.PlatformExists(platformId)) return NotFound();

        var command = repository.GetCommand(platformId, commandId);
        if (command == null) return NotFound();

        var dto = mapper.Map<CommandResponse>(command);
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<CommandResponse> CreateCommandForPlatform(int platformId, CommandRequest request)
    {
        if (!repository.PlatformExists(platformId)) return NotFound();

        var command = mapper.Map<Command>(request);
        repository.CreateCommand(platformId, command);
        repository.SaveChanges();
        
        var response = mapper.Map<CommandResponse>(command);
        return CreatedAtRoute(
            nameof(GetCommandForPlatform), 
            new {platformId, commandId = response.Id,},
            response);
    }
}
