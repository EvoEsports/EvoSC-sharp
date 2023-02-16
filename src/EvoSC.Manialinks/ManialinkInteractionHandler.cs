using EvoSC.Common.Events;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote;
using EvoSC.Manialinks.Interfaces;
using GbxRemoteNet.Events;

namespace EvoSC.Manialinks;

public class ManialinkInteractionHandler : IManialinkInteractionHandler
{
    public ManialinkInteractionHandler(IEventManager events)
    {
        events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.ManialinkPageAnswer)
            .WithInstance(this)
            .WithInstanceClass<ManialinkInteractionContext>()
            .WithHandlerMethod<ManiaLinkPageActionEventArgs>(HandleManialinkPageAnswerAsync)
            .AsAsync()
        );
    }

    private async Task HandleManialinkPageAnswerAsync(object? sender, ManiaLinkPageActionEventArgs args)
    {
        var (action, pars) = await ParseAnswerAsync(args.Answer);
    }

    private async Task<(string, KeyValuePair<String, object>[])> ParseAnswerAsync(string answerString)
    {
        return ("", Array.Empty<KeyValuePair<String, object>>());
    }
}
