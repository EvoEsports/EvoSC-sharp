using EvoSC.Core.Events.Callbacks.Args;
using EvoSC.Interfaces.Players;
using PluginSample.Interfaces;

namespace PluginSample.Events;

public class PlayerEventHandler : IPlayerEventHandler
{
    private readonly IPlayerCallbacks _playerCallbacks;

    public PlayerEventHandler(IPlayerCallbacks playerCallbacks)
    {
        _playerCallbacks = playerCallbacks;
    }

    public void HandleEvents()
    {
        _playerCallbacks.PlayerConnect += PlayerCallbacksOnPlayerConnect;
    }

    private void PlayerCallbacksOnPlayerConnect(object? sender, PlayerConnectEventArgs e)
    {
        Console.WriteLine("Fired playerConnect");
    }
}
