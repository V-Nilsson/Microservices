using PlatformService.Contract;

namespace PlatformService.AsyncDataServices;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublished platform);
}
