using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Contract;
using PlatformService.Data;
using PlatformService.Models;

namespace PlatformService.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _platformRepository;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepository platformRepository, IMapper mapper)
    {
        _platformRepository = platformRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformResponse>> GetPlatforms()
    {
        var platforms = _platformRepository.GetAllPlatforms();
        var dto = _mapper.Map<IEnumerable<PlatformResponse>>(platforms);
        return Ok(dto);
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformResponse> GetPlatformById(int id)
    {
        var platform = _platformRepository.GetPlatformById(id);
        if (platform == null) return NotFound();
        var dto = _mapper.Map<PlatformResponse>(platform);
        return Ok(dto);
    }

    [HttpPost]
    public ActionResult<PlatformResponse> CreatePlatform(PlatformRequest request)
    {
        var platformModel = _mapper.Map<Platform>(request);
        _platformRepository.CreatePlatform(platformModel);
        _platformRepository.SaveChanges();

        var response = _mapper.Map<PlatformResponse>(platformModel);
        return CreatedAtRoute(nameof(GetPlatformById), new { response.Id }, response);
    }
}
