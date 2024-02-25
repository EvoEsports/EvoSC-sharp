using System.Dynamic;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.SetName.Controllers;
using EvoSC.Modules.Official.SetName.Interfaces;
using EvoSC.Modules.Official.SetName.Models;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace EvoSC.Modules.Official.SetName.Tests;

public class SetNameControllerTests : ManialinkControllerTestBase<SetNameController>
{
    private IManialinkActionContext _manialinkActionContext = Substitute.For<IManialinkActionContext>();
    private IOnlinePlayer _actor = Substitute.For<IOnlinePlayer>();
    private ISetNameService _setNameService = Substitute.For<ISetNameService>();
    private Locale _locale;

    public SetNameControllerTests()
    {
        _locale = Mocking.NewLocaleMock(ContextService);
        
        InitMock(_actor, _manialinkActionContext, _setNameService, _locale);
    }

    [Fact]
    public async Task Edit_Name_Action_Is_Success()
    {
        var entry = new SetNameEntryModel {Nickname = "MyNickname"};
        _manialinkActionContext.EntryModel.Returns(entry);

        await Controller.ValidateModelAsync();
        await Controller.EditNameAsync(entry);

        await _setNameService.Received(1).SetNicknameAsync(_actor, entry.Nickname);
        await ManialinkManager.Received(1).HideManialinkAsync(_actor, "SetName.EditName");
        AuditEventBuilder.Received(1).Success();
        AuditEventBuilder.Received(1).WithEventName("EditNickname");
    }

    [Fact]
    public async Task Invalid_Entry_Model_Shows_Manialink_Again()
    {
        var entry = new SetNameEntryModel {Nickname = ""};
        _manialinkActionContext.EntryModel.Returns(entry);
        await Controller.ValidateModelAsync();
        await Controller.EditNameAsync(entry);
        
        await _setNameService.DidNotReceive().SetNicknameAsync(_actor, entry.Nickname);
        await ManialinkManager.DidNotReceive().SendManialinkAsync(_actor, "SetName.EditName", Arg.Any<ExpandoObject>());
    }

    [Fact]
    public async Task SetName_Is_Canceled()
    {
        await Controller.CancelAsync();
        
        await ManialinkManager.Received().HideManialinkAsync(_actor, "SetName.EditName");
    }
}
