using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EvoSC.Core.Commands.Generic.Interfaces;
using GbxRemoteNet;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

namespace EvoSC.Core.Commands.Generic;

public class Command : ICommand
{
    public MethodInfo CmdMethod { get; private set; }
    public ICommandParameter[] Parameters { get; private set; }
    public Type GroupType { get; private set; }
    public string Description { get; private set; }
    public string Name { get; private set; }
    public string? Permission { get; private set; }
    public string? Group { get; private set; }

    public Command(MethodInfo methodInfo, Type groupType, IEnumerable<ICommandParameter> pars, string name,
        string description, string? permission = null, string? group = null)
    {
        CmdMethod = methodInfo;
        GroupType = groupType;
        Description = description;
        Name = name;
        Permission = permission;
        Parameters = pars.ToArray();
        Group = group;
    }

    public void SetGroup(string groupName)
    {
        Group = groupName;
    }

    public async Task<ICommandResult> Invoke(IServiceProvider services, ICommandContext context, params object[] args)
    {
        var instance = (ICommandGroup)ActivatorUtilities.CreateInstance(services, GroupType);

        if (instance == null)
        {
            return new CommandResult(false, new InvalidOperationException("Could not create command group instance."));
        }

        instance.SetContext(context);

        try
        {
            await (Task)CmdMethod.Invoke(instance, args);

            return new CommandResult(true);
        }
        catch (Exception ex)
        {
            return new CommandResult(false, ex);
        }
    }

    public int RequiredParameters() => Parameters.Count(p => !p.Optional);
}
