using EvoSC.Domain.Groups;

namespace EvoSC.Core.Commands.Generic.Interfaces;

public interface ICommandGroupInfo
{
    public string Name { get; }
    public string Description { get; }
    public string? Permission { get; set; }
}
