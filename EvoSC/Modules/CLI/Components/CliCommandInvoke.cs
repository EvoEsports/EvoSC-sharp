using System;
using DefaultEcs;

namespace EvoSC.Modules.CLI.Components
{
    public delegate bool CliCommandInvoke(Span<Entity> args);
}