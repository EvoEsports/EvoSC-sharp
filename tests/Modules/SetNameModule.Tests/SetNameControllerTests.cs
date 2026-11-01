using System.Dynamic;
using EvoSC.Common.Interfaces.Localization;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Manialinks.Interfaces.Models;
using EvoSC.Modules.Official.SetNameModule.Controllers;
using EvoSC.Modules.Official.SetNameModule.Interfaces;
using EvoSC.Modules.Official.SetNameModule.Models;
using EvoSC.Testing;
using EvoSC.Testing.Controllers;
using Moq;

namespace EvoSC.Modules.Official.SetName.Tests;

public class SetNameControllerTests : ManialinkControllerTestBase<SetNameController>
{
    private Mock<IManialinkActionContext> _manialinkActionContext = new();
    private Mock<IOnlinePlayer> _actor = new();
    private Mock<ISetNameService> _setNameService = new();
    private Locale _locale;

    public SetNameControllerTests()
    {
        _locale = Mocking.NewLocaleMock(ContextService.Object);
        
        InitMock(_actor.Object, _manialinkActionContext.Object, _setNameService, _locale);
    }

    [Fact]
    public async Task Edit_Name_Action_Is_Success()
    {
        var entry = new SetNameEntryModel {Nickname = "MyNickname"};
        _manialinkActionContext.Setup(m => m.EntryModel).Returns(entry);

        await Controller.ValidateModelAsync();
        await Controller.EditNameAsync(entry);

        _setNameService.Verify(m => m.SetNicknameAsync(_actor.Object, entry.Nickname), Times.Once);
        ManialinkManager.Verify(m => m.HideManialinkAsync(_actor.Object, "SetNameModule.EditName"), Times.Once);
        AuditEventBuilder.Verify(m => m.Success(), Times.Once);
        AuditEventBuilder.Verify(m => m.WithEventName("EditNickname"), Times.Once);
    }

    [Fact]
    public async Task Invalid_Entry_Model_Shows_Manialink_Again()
    {
        var entry = new SetNameEntryModel {Nickname = ""};
        _manialinkActionContext.Setup(m => m.EntryModel).Returns(entry);
        await Controller.ValidateModelAsync();
        await Controller.EditNameAsync(entry);
        
        _setNameService.Verify(m => m.SetNicknameAsync(_actor.Object, entry.Nickname), Times.Never);
        ManialinkManager.Verify(m => m.SendManialinkAsync(_actor.Object, "SetNameModule.EditName", It.IsAny<ExpandoObject>()),
            Times.Once);
    }

    [Fact]
    public async Task SetName_Is_Canceled()
    {
        await Controller.CancelAsync();
        
        ManialinkManager.Verify(m => m.HideManialinkAsync(_actor.Object, "SetNameModule.EditName"));
    }
}
