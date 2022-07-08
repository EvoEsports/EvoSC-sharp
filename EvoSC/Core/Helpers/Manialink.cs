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
    private string _actionClose;

    public Manialink(GbxRemoteClient gbxRemoteClient)
    {
        _gbxRemoteClient = gbxRemoteClient;
    }

    public async Task Send(DatabasePlayer databasePlayer)
    {
        try
        {
            var action = new ManialinkAction(async (action) => await Hide(databasePlayer));
            _actionClose = UiService.RegisterAction(action);
            var template = new TemplateEngine(@"templates", "test.xml");
            var xml = template
                .Render(new
                {
                    title = "testWindow 1",
                    name = "test1",
                    size = "120 60",
                    pos = "0 0",
                    id = "test1",
                    closeaction = _actionClose,
                    items = "Race|Tech|FullSpeed|Speed fun",
                }).Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", string.Empty);
            var template2 = new TemplateEngine(@"templates", "test.xml");
            var xml2 = template2
                .Render(new
                {
                    title = "testWindow2",
                    id = "test2",
                    name = "test2",
                    size = "120 60",
                    pos = "30 -30",
                    closeaction = _actionClose,
                    items = "Race|Tech|FullSpeed|Speed fun",
                }).Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", string.Empty);
            var outXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><manialinks>" + xml + xml2 + "</manialinks>";
            await _gbxRemoteClient.SendDisplayManialinkPageToLoginAsync(databasePlayer.Login, outXml, 0, false);
        }
        catch (Exception e)
        {
            _logger.Error(e);
        }
    }

    public async Task Hide(DatabasePlayer databasePlayer)
    {
        await _gbxRemoteClient.SendHideManialinkPageToLoginAsync(databasePlayer.Login);
        UiService.UnregisterAction(_actionClose);
    }
}
