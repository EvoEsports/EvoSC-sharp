using System;
using DefaultEcs;

namespace EvoSC.CLI
{
    public delegate bool CliCommandInvoke(Span<Entity> args);
}
