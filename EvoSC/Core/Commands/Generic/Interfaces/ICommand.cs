using System;
using System.Reflection;
using System.Threading.Tasks;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommand
{
    public MethodInfo CmdMethod { get; }
    public ParameterInfo[] Parameters { get; }
    public Type GroupType { get; }
    public string Description { get; }
    public string Name { get; }
    public string? Permission { get; }
    public string? Group { get; }

    public void SetGroup(string groupName);
    public Task<ICommandResult> Invoke(IServiceProvider services, ICommandContext context, params object[] args);
}
