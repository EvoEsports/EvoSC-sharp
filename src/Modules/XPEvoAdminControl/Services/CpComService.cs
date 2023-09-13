using System.Text.Json;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Remote;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Modules.Official.XPEvoAdminControl.Events;
using EvoSC.Modules.Official.XPEvoAdminControl.Events.EventArgs;
using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces;
using EvoSC.Modules.Official.XPEvoAdminControl.Interfaces.CpCom;
using EvoSC.Modules.Official.XPEvoAdminControl.Models.CpCom;
using GbxRemoteNet.Events;

namespace EvoSC.Modules.Official.XPEvoAdminControl.Services;

[Service(LifeStyle = ServiceLifeStyle.Singleton)]
public class CpComService : ICpComService
{
    public const string ActionPrefix = "XPEvoAdminAction";
    
    private readonly IServerClient _server;
    private readonly IEventManager _events;
    
    public CpComService(IServerClient server, IEventManager events)
    {
        _server = server;
        _events = events;
        
        _events.Subscribe(s => s
            .WithEvent(GbxRemoteEvent.Echo)
            .WithInstance(this)
            .WithInstanceClass<CpComService>()
            .WithHandlerMethod<EchoGbxEventArgs>(OnEcho)
            .AsAsync()
        );
    }
    
    public Task RespondAsync(ICpAction action, Guid actionId)
    {
        action.ActionId = actionId;
        var packet = JsonSerializer.Serialize((object)action);
        return _server.Remote.EchoAsync($"XPEvoAdminAction.Response", packet);
    }
    
    public Task UpdateAsync(ICpAction action)
    {
        var packet = JsonSerializer.Serialize((object)action);
        return _server.Remote.EchoAsync($"XPEvoAdminAction.Update", packet);
    }

    public Task RespondErrorAsync(string message, Guid actionId) =>
        RespondAsync(new CpAction
        {
            AccessToken = null, 
            Action = $"{ActionPrefix}.Response", 
            Data = new
            {
                Success = false,
                ErrorMessage = message
            }
        }, actionId);

    public Task RespondSuccessAsync(Guid actionId) =>
        RespondAsync(new CpAction
        {
            AccessToken = null,
            Action = $"{ActionPrefix}.Response", 
            Data = new { Success = true }
        }, actionId);
    
    
    private async Task OnEcho(object sender, EchoGbxEventArgs args)
    {
        if (!args.PublicParam.StartsWith(ActionPrefix) || args.PublicParam.Length <= ActionPrefix.Length)
        {
            return;
        }

        var action = args.PublicParam.Substring(ActionPrefix.Length + 1);

        if (action == "Response" || action == "Update")
        {
            return;
        }
        
        var actionInfo = JsonSerializer.Deserialize<CpAction>(args.InternalParam);

        await _events.RaiseAsync(AdminCpEvents.Action, new CpComActionEventArgs { Action = actionInfo }, this);
    }
}
