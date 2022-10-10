
using EvoSC.Common.Interfaces;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.Builtin.Player;

[InternalModule(Name, "Module to handle players.")]
public class PlayerModule : EvoScModule
{
    public const string Name = "Player";

    public override Type[] Controllers => new[] {typeof(PlayerController)};
}
