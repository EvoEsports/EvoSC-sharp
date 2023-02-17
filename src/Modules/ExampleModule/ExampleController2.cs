using EvoSC.Commands;
using EvoSC.Commands.Attributes;
using EvoSC.Common.Controllers;
using EvoSC.Common.Controllers.Attributes;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Util;
using EvoSC.Modules.Interfaces;

namespace EvoSC.Modules.Official.ExampleModule;

[Controller]
public class ExampleController2 : EvoScController<CommandInteractionContext>
{
    private readonly IServerClient _server;
    
    public ExampleController2(IServerClient server)
    {
        _server = server;
    }

    [ChatCommand("show", "Show a manialink")]
    public async Task ShowManialink()
    {
        var manialink = """
<?xml version="1.0" encoding="utf-8" standalone="yes" ?>
<manialink version="3" name="my-manialink">
    <textedit name="MyValue" pos="30 0" size="100 20" textformat="Script" textsize="1" showlinenumbers="1" />
    <label pos="0 -5" text="submit" action="ExampleManialink/HandleAction/1" />
</manialink>
""";

        await _server.Remote.SendDisplayManialinkPageToLoginAsync(Context.Player.GetLogin(), manialink, 0, false);
    }
    
    [ChatCommand("hide", "Hide a manialink")]
    public async Task HideManialink()
    {
        await _server.Remote.SendHideManialinkPageAsync();
    }
}
