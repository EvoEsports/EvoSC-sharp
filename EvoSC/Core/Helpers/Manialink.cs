using System;
using System.Threading.Tasks;
using EvoSC.Core.Services.UI;
using EvoSC.Domain.Players;
using GbxRemoteNet;
using NLog;

namespace EvoSC.Core.Helpers;

public class Manialink
{
    private readonly GbxRemoteClient _gbxRemoteClient;
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();

    public Manialink(GbxRemoteClient gbxRemoteClient)
    {
        _gbxRemoteClient = gbxRemoteClient;
    }

    public async Task Send(Player player)
    {
        try
        {
            var template = new TemplateEngine(@"templates", "test.xml");
            var xml = template.Render(new {title = "test", name = "test", size="200 200"});
            xml = template.Render(new {title = "asd", name = "asd", size="100 100"});
            await _gbxRemoteClient.SendDisplayManialinkPageToLoginAsync(player.Login, xml, 0, false);
            xml = null;
        }
        catch (Exception e)
        {
            _logger.Error(e);
        }
        
    }
}
