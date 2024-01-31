using AutoMapper;
using PlatformService.Contract;
using PlatformService.Models;

namespace PlatformService.Profiles;

public class PlatformProfile : Profile
{
    public PlatformProfile()
    {
        CreateMap<Platform, PlatformResponse>();
        CreateMap<PlatformResponse, Platform>();
        CreateMap<PlatformRequest, Platform>();
    }
}
