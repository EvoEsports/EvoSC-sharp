﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Generic;
using EvoSC.Core.Commands.Generic.Interfaces;
using EvoSC.Domain.Players;
using EvoSC.Interfaces.Messages;
using EvoSC.Interfaces.Players;

namespace EvoSC.Interfaces.Commands;

public interface ICommandsService
{
    public CommandCollection Commands { get; }

    /// <summary>
    /// Register commands from a commands group's type.
    /// </summary>
    /// <param name="type"></param>
    public Task RegisterCommands(Type type);

    /// <summary>
    /// Register commands from a commands group's type.
    /// </summary>
    /// <param name="type"></param>
    public Task RegisterCommands<T>();

    /// <summary>
    /// Remove commands of a given commands group's type.
    /// </summary>
    /// <param name="type"></param>
    public Task UnregisterCommands(Type type);

    /// <summary>
    /// Remove commands of a given commands group's type.
    /// </summary>
    /// <param name="type"></param>
    public Task UnregisterCommands<T>();

    /// <summary>
    /// Execute a command based on the parser result.
    /// </summary>
    /// <param name="parserResult"></param>s
    /// <returns></returns>
    public Task<ICommandResult> ExecuteCommand(ICommandContext context, ICommandParserResult parserResult);

    /// <summary>
    /// Get a command from it's name.
    /// </summary>
    /// <param name="group"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public ICommand GetCommand(string group, string? name = null);
}