﻿using CommandsService.Models;
using System;

namespace CommandsService.Data;

public class CommandRepository : ICommandRepository
{
    private readonly ApplicationDbContext dbContext;

    public CommandRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateCommand(int platformId, Command command)
    {
        if (command == null) throw new ArgumentException(nameof(command));

        command.PlatformId = platformId;
        dbContext.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform == null) throw new ArgumentException(nameof(platform));

        dbContext.Platforms.Add(platform);
    }

    public bool ExternalPlatformExists(int externalPlatformId)
    {
        return dbContext.Platforms.Any(p => p.ExternalId == externalPlatformId);

    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return dbContext.Platforms.ToList();
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return dbContext.Commands.FirstOrDefault(c => c.Id == commandId && c.PlatformId == platformId);
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return dbContext.Commands.Where(c =>  c.PlatformId == platformId);
    }

    public bool PlatformExists(int platformId)
    {
        return dbContext.Platforms.Any(p => p.Id == platformId);
    }

    public bool SaveChanges()
    {
        return (dbContext.SaveChanges() >= 0);
    }
}
