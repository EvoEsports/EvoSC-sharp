using System;
using EvoSC.Core.Plugins;

namespace EvoSC.Core.Modules.Info;

public class Info : EvoSCPlugin
{
    public Info(IChatCommandsService commands)
    {
        commands.RegisterCommands<Commands>();
    }
}
