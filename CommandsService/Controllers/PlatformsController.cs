using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;
[Route("api/commandsservice/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    public PlatformsController()
    {
        
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command Service");
        return Ok(0);
    }

    [HttpGet]
    public ActionResult TestConnection()
    {
        Console.WriteLine("This is a test of Docker connection");
        return Ok(1);
    }
}