using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvoSC.Common.Config.Models;
using EvoSC.Common.Interfaces;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Interfaces.Themes;
using EvoSC.Common.Themes;
using EvoSC.Common.Themes.Attributes;
using EvoSC.Common.Themes.Events;
using EvoSC.Common.Themes.Events.Args;
using EvoSC.Common.Themes.Exceptions;
using Moq;
using SimpleInjector;
using Xunit;

namespace EvoSC.Common.Tests.Themes;

public class ThemeManagerTests
{
    [Theme(Name = "MyTheme", Description = "This is my theme.")]
    public class MyTheme : Theme<MyTheme>
    {
        public override Task ConfigureAsync()
        {
            Set("MyThemeOption").To("MyThemeOptionValue");
            Replace("MyComponent").With("MyOtherComponent");
            return Task.CompletedTask;
        }
    }

    [Theme(Name = "MyTheme2", Description = "This is my second theme.")]
    public class MyTheme2 : Theme<MyTheme2>
    {
        public override Task ConfigureAsync()
        {
            Set("MyThemeOption2").To("MyThemeOptionValue2");
            return Task.CompletedTask;
        }
    }
    
    [Theme(Name = "MyTheme3", Description = "This is my third theme.")]
    public class MyTheme3 : Theme<MyTheme3>
    {
        public override Task ConfigureAsync()
        {
            Set("MyThemeOption3").To("MyThemeOptionValue3");
            return Task.CompletedTask;
        }
    }
    
    [Theme(Name = "MyThemeOverride", Description = "This is my theme override.", OverrideTheme = typeof(MyTheme))]
    public class MyThemeOverride : Theme<MyThemeOverride>
    {
        public override Task ConfigureAsync()
        {
            Set("MyThemeOption").To("NewMyThemeOptionValue");
            return Task.CompletedTask;
        }
    }
    
    public class InvalidTheme : Theme<InvalidTheme>
    {
        public override Task ConfigureAsync() => Task.CompletedTask;
    }

    private (
        Mock<IServiceContainerManager> ServiceManager,
        Mock<IEvoSCApplication> EvoSCApp,
        Mock<IEventManager> Events,
        Mock<IEvoScBaseConfig> Config,
        IThemeManager ThemeManager
        ) GetMock()
    {
        var serviceManager = new Mock<IServiceContainerManager>();
        var app = new Mock<IEvoSCApplication>();
        var events = new Mock<IEventManager>();
        var config = new Mock<IEvoScBaseConfig>();

        app.Setup(p => p.Services).Returns(new Container());

        config.Setup(p => p.Theme).Returns(new DynamicThemeOptions(new Dictionary<string, object>
        {
            { "MyOptions.MyOption1", "MyValue" },
            { "MyOptions.MyOption2", "MyValue2" },
            { "MyOptions.MyOption3", "MyValue3" }
        }));
        
        var manager = new ThemeManager(serviceManager.Object, app.Object, events.Object, config.Object);

        return (
            serviceManager,
            app,
            events,
            config,
            manager
        );
    }
    
    [Fact]
    public async Task Default_Theme_Falls_Back_To_Config()
    {
        var mock = GetMock();
        
        Assert.Equal("MyValue", mock.ThemeManager.Theme.MyOptions_MyOption1);
        Assert.Equal("MyValue2", mock.ThemeManager.Theme.MyOptions_MyOption2);
        Assert.Equal("MyValue3", mock.ThemeManager.Theme.MyOptions_MyOption3);
    }

    [Fact]
    public async Task New_Theme_Added_Is_Auto_Activated()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        
        Assert.Equal("MyThemeOptionValue", mock.ThemeManager.Theme.MyThemeOption);
    }

    [Fact]
    public async Task Overriding_Theme_Is_Not_Activated_Automatically()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        await mock.ThemeManager.AddThemeAsync(typeof(MyThemeOverride));
        
        Assert.Equal("MyThemeOptionValue", mock.ThemeManager.Theme.MyThemeOption);
    }
    
    [Fact]
    public async Task Theme_Override_Replaces_Existing_Theme()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        await mock.ThemeManager.AddThemeAsync(typeof(MyThemeOverride));

        await mock.ThemeManager.ActivateThemeAsync("MyThemeOverride");
        
        Assert.Equal("NewMyThemeOptionValue", mock.ThemeManager.Theme.MyThemeOption);
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
        Assert.Equal(new[]{"MyTheme", "MyTheme2", "MyTheme3"}, themes);
        Assert.Equal("MyThemeOptionValue", mock.ThemeManager.Theme.MyThemeOption);
        Assert.Equal("MyThemeOptionValue2", mock.ThemeManager.Theme.MyThemeOption2);
        Assert.Equal("MyThemeOptionValue3", mock.ThemeManager.Theme.MyThemeOption3);
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
            await mock.ThemeManager.ActivateThemeAsync("DoesNotExist");
        });
    }
    
    [Fact]
    public async Task Theme_Is_Removed()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme2));

        await mock.ThemeManager.ActivateThemeAsync("MyTheme2");
        await mock.ThemeManager.RemoveThemeAsync("MyTheme");

        var themes = mock.ThemeManager.AvailableThemes.Select(t => t.Name);
        
        Assert.DoesNotContain("MyTheme", themes);
    }

    [Fact]
    public async Task On_Theme_Changed_Event_Is_Fired()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));
        
        mock.Events.Verify(e => e.RaiseAsync(ThemeEvents.CurrentThemeChanged, It.IsAny<ThemeUpdatedEventArgs>()));
    }

    [Fact]
    public async Task Component_Replacements_Are_Added()
    {
        var mock = GetMock();

        await mock.ThemeManager.AddThemeAsync(typeof(MyTheme));

        var replacements = mock.ThemeManager.ComponentReplacements;

        Assert.True(replacements.ContainsKey("MyComponent"));
        Assert.Equal("MyOtherComponent", replacements["MyComponent"]);
    }
}
