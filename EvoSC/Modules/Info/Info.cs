using System;
using EvoSC.Core.Plugins;

namespace EvoSC.Modules.Info;

public class Info : EvoSCPlugin
{
    public Info(IChatCommandsService commands)
    {
        commands.RegisterCommands<Commands>();
    }
}
