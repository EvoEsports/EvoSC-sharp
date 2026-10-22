using EvoSC.Commands;
using EvoSC.Commands.Interfaces;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Official.ShowCommandsModule.Controllers;
using EvoSC.Modules.Official.ShowCommandsModule.Models;
using EvoSC.Testing.Controllers;
using Moq;

namespace ShowCommandsModule.Tests;

public class ShowCommandsControllerTests : CommandInteractionControllerTestBase<ShowCommandsController>
{
    private const string TemplateName = "ShowCommandsModule.ShowCommandsTemplate";

    private readonly Mock<IChatCommandManager> _commands = new();
    private readonly Mock<IManialinkManager> _manialinks = new();
    private readonly Mock<IOnlinePlayer> _actor = new();

    private object? _sentData;

    public ShowCommandsControllerTests()
    {
        _manialinks
            .Setup(m => m.SendManialinkAsync(It.IsAny<IPlayer>(), It.IsAny<string>(), It.IsAny<object>()))
            .Callback<IPlayer, string, object>((_, _, data) => _sentData = data)
            .Returns(Task.CompletedTask);

        InitMock(_actor.Object, _commands.Object, _manialinks.Object);
    }

    private static Mock<IChatCommand> NewCommand(string name, string description = "", string? permission = null,
        bool usePrefix = true, params (string Name, bool Optional)[] parameters)
    {
        var cmd = new Mock<IChatCommand>();
        cmd.Setup(c => c.Name).Returns(name);
        cmd.Setup(c => c.Description).Returns(description);
        cmd.Setup(c => c.Permission).Returns(permission);
        cmd.Setup(c => c.UsePrefix).Returns(usePrefix);
        cmd.Setup(c => c.Parameters).Returns(parameters.Select(NewParameter).ToArray());
        return cmd;
    }

    private static ICommandParameter NewParameter((string Name, bool Optional) p)
    {
        var param = new Mock<ICommandParameter>();
        param.Setup(x => x.Name).Returns(p.Name);
        param.Setup(x => x.Optional).Returns(p.Optional);
        return param.Object;
    }

    private async Task<CommandInfo[]> GetSentCommandsAsync(params IChatCommand[] registered)
    {
        _commands.Setup(c => c.GetCommands()).Returns(registered);

        await Controller.ShowCommandsAsync();

        Assert.NotNull(_sentData);
        return (CommandInfo[])_sentData.GetType().GetProperty("commands")!.GetValue(_sentData)!;
    }

    [Fact]
    public async Task ShowCommandsAsync_Sends_Template_To_Player()
    {
        await GetSentCommandsAsync();

        _manialinks.Verify(m => m.SendManialinkAsync(_actor.Object, TemplateName, It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ShowCommandsAsync_Maps_Description_And_Permission()
    {
        var result = await GetSentCommandsAsync(NewCommand("kick", "Kick a player", "Admin").Object);

        var info = Assert.Single(result);
        Assert.Equal("Kick a player", info.Description);
        Assert.Equal("Admin", info.Permission);
    }

    [Fact]
    public async Task ShowCommandsAsync_Prefixes_Usage_When_Command_Uses_Prefix()
    {
        var result = await GetSentCommandsAsync(NewCommand("maps").Object);

        Assert.Equal($"{ChatCommandManager.CommandPrefix}maps", Assert.Single(result).Usage);
    }

    [Fact]
    public async Task ShowCommandsAsync_Omits_Prefix_When_Command_Does_Not_Use_Prefix()
    {
        var result = await GetSentCommandsAsync(NewCommand("gg", usePrefix: false).Object);

        Assert.Equal("gg", Assert.Single(result).Usage);
    }

    [Fact]
    public async Task ShowCommandsAsync_Formats_Required_And_Optional_Parameters()
    {
        var cmd = NewCommand("ban", parameters: [("player", false), ("reason", true)]);

        var result = await GetSentCommandsAsync(cmd.Object);

        Assert.Equal($"{ChatCommandManager.CommandPrefix}ban <player> [reason]", Assert.Single(result).Usage);
    }

    [Fact]
    public async Task ShowCommandsAsync_Sorts_Commands_Without_Permission_First_Then_By_Usage()
    {
        var result = await GetSentCommandsAsync(
            NewCommand("zeta", permission: "Admin").Object,
            NewCommand("beta").Object,
            NewCommand("alpha", permission: "Admin").Object,
            NewCommand("aardvark").Object);

        Assert.Equal(
            ["/aardvark", "/beta", "/alpha", "/zeta"],
            result.Select(c => c.Usage));
    }

    [Fact]
    public async Task ShowCommandsAsync_Sends_Empty_List_When_No_Commands_Registered()
    {
        Assert.Empty(await GetSentCommandsAsync());
    }
}
