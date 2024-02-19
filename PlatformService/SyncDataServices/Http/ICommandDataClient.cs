using PlatformService.Contract;

namespace PlatformService.SyncDataServices.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformResponse platformResponse);
}
