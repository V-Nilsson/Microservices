using AutoMapper;
using CommandsService.Contract;
using CommandsService.Models;

namespace CommandsService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformResponse>();
        CreateMap<CommandRequest, Command>();
        CreateMap<Command, CommandResponse>();
        CreateMap<PlatformPublished, Platform>()
            .ForMember(destination => destination.ExternalId, options => options.MapFrom(src => src.Id));
    }
}
