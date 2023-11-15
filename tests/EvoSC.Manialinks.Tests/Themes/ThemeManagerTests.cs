using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Manialinks.Attributes;
using EvoSC.Manialinks.Exceptions.Themes;
using EvoSC.Manialinks.Interfaces.Themes;
using EvoSC.Manialinks.Themes;
using EvoSC.Manialinks.Themes.Events;
using EvoSC.Manialinks.Themes.Events.Args;
using Moq;

namespace EvoSC.Manialinks.Tests.Themes;

public class ThemeManagerTests
{
    [Theme(Name = "MyTheme", Description = "This is my theme.")]
    public class MyTheme : Theme<MyTheme>
    {
        public override Task ConfigureAsync()
        {
            return Task.CompletedTask;
        }
    }
    
    [Theme(Name = "MyTheme2", Description = "This is my second theme.")]
    public class MyTheme2 : Theme<MyTheme>
    {
        public override Task ConfigureAsync()
        {
            return Task.CompletedTask;
        }
    }
    
    [Theme(Name = "MyTheme3", Description = "This is my third theme.")]
    public class MyTheme3 : Theme<MyTheme>
    {
        public override Task ConfigureAsync()
        {
            return Task.CompletedTask;
        }
    }
    
    public class InvalidTheme : Theme<MyTheme>
    {
        public override Task ConfigureAsync()
        {
            return Task.CompletedTask;
        }
    }

    private (
        Mock<IServiceContainerManager> ServiceManager,
        Mock<IEvoSCApplication> EvoSCApp,
        Mock<IEventManager> Events,
        IThemeManager ThemeManager
        ) GetMock()
    {
        var serviceManager = new Mock<IServiceContainerManager>();
        var app = new Mock<IEvoSCApplication>();
        var events = new Mock<IEventManager>();
        var manager = new ThemeManager(serviceManager.Object, app.Object, events.Object);

        return (
            serviceManager,
            app,
            events,
            manager
        );
    }

    [Fact]
    public async Task First_Theme_Added_Is_Set_As_Current()
    {
        var mock = GetMock();
        
        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        
        Assert.NotNull(mock.ThemeManager.CurrentTheme);
        Assert.IsType<MyTheme>(mock.ThemeManager.CurrentTheme);
    }

    [Fact]
    public async Task Available_Themes_Property_Returns_All_Themes()
    {
        var mock = GetMock();
        
        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme2));
        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme3));

        var themes = mock.ThemeManager.AvailableThemes.Select(t => t.Name).ToArray();
        
        Assert.NotEmpty(themes);
        Assert.Equal(new string[]{"MyTheme", "MyTheme2", "MyTheme3"}, themes);
    }

    [Fact]
    public async Task Theme_Is_Activated_And_Set_As_Current()
    {
        var mock = GetMock();
        
        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme2));
        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme3));

        await mock.ThemeManager.SetCurrentThemeAsync("MyTheme2");
        
        Assert.NotNull(mock.ThemeManager.CurrentTheme);
        Assert.IsType<MyTheme2>(mock.ThemeManager.CurrentTheme);
    }

    [Fact]
    public async Task Adding_Existing_Theme_Throws_Exception()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));

        await Assert.ThrowsAsync<ThemeException>(async () =>
        {
            await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        });
    }

    [Fact]
    public async Task Adding_Theme_Without_Attribute_Throws_Exception()
    {
        var mock = GetMock();

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await mock.ThemeManager.AddThemeAsync(typeof(InvalidTheme));
        });
    }

    [Fact]
    public async Task Throws_Exception_When_Accessing_NonExistent_Theme()
    {
        var mock = GetMock();

        await Assert.ThrowsAsync<ThemeDoesNotExistException>(async () =>
        {
            await mock.ThemeManager.SetCurrentThemeAsync("DoesNotExist");
        });
    }

    [Fact]
    public async Task Removing_Current_Theme_Fails()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));

        Assert.Throws<ThemeException>(() =>
        {
            mock.ThemeManager.RemoveTheme("MyTheme");
        });
    }

    [Fact]
    public async Task Theme_Is_Removed()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme2));

        await mock.ThemeManager.SetCurrentThemeAsync("MyTheme2");
        mock.ThemeManager.RemoveTheme("MyTheme");

        var themes = mock.ThemeManager.AvailableThemes.Select(t => t.Name);
        
        Assert.DoesNotContain("MyTheme", themes);
    }

    [Fact]
    public async Task On_Theme_Changed_Event_Is_Fired()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        
        mock.Events.Verify(e => e.RaiseAsync(ThemeEvents.CurrentThemeChanged, It.IsAny<ThemeChangedEventArgs>()));
    }
}
