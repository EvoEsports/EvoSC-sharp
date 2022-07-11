using System;
using System.Reflection;
using System.Threading.Tasks;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommand
{
    public MethodInfo CmdMethod { get; }
    public ICommandParameter[] Parameters { get; }
    public Type GroupType { get; }
    public string Description { get; }
    public string Name { get; }
    public string? Permission { get; }
    public ICommandGroupInfo? Group { get; }

    public Task<ICommandResult> Invoke(IServiceProvider services, ICommandContext context, params object[] args);
    public int RequiredParameters();
}
