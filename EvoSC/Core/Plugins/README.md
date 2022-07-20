# EvoSC Plugin System

There are two types of plugins, internal and external.

Internal plugins are defined within the main application assembly and are called modules. External plugins are additional 
functionality that can be added in the plugins folder.

## Internal Plugins
All internal plugins should be defined under the `EvoSC.Modules` namespace. Each module should also have its own namespace, so the
namespace for each plugin should be as following: `EvoSC.Modules.<module-name>`.

## External Plugins
Each external plugin should lie within the `ext_plugins` directory relative to the application's working directory. Each of the plugins must have
their own directory where the plugin files are.

Each plugin must also have a `info.toml` file that holds info various info about the plugin like name, description, version, author, dependencies etc.

### `info.toml` Format

```toml
[info]
# Name of the plugin, must be unique and alphanumeric
name = "ExamplePlugin"
# Title of the plugin, can be almost anything
title = "Example Plugin"
# The plugin version must be in the SemVer format
version = "1.0.0"
# Name of the author that developed the plugin
author = "Evo"
# A short description of the plugin that describes what it is and what it does.
summary = "An example plugin to show how it all works."

# Dependencies are defined by the name of the plugin required and the version required
# Be aware of dependency cycles as plugins will refuse to load if this is detected.
[dependencies]
plugin1 = "1.0.0"
plugin2 = "1.0.0"
plugin3 = "1.0.0"
# ...
```

## Plugin Class
Each module should have a plugin class defined that extends the `EvoSCPlugin` abstract class.

For example the absolute minimum definition of a plugin is the following:
```csharp
using EvoSC.Core.Plugins;
using Microsoft.Extensions.DependencyInjection;

// the namespace must be `EvoSC.Modules.ExamplePlugin` if this is an internal plugin
namespace ExamplePlugin;

// If you want more control over what happens
public class ExamplePlugin : EvoSCPlugin
{
}
```

### `IPlugin` and `EvoSCPlugin`
The `IPlugin` interface is the absolute base of any plugin. `EvoSCPlugin` is an abstract class that provides a bit of automatic setup of some background things
and provides some utilities and properties that makes things easier. However, if you need absolute control over what happens when a plugin is instantiated
you can also just implement `IPlugin` directly.

**It is, however, recommended to always inherit `EvoSCPlugin`.**

### The `Setup` static method
If you declare a special method with the signature within the plugin class:
```csharp
public static void Setup(IServiceCollection services)
```
It is possible to add services to the plugin's service provider which will be available through dependency injection later on. Any plugin that depend
on your plugin will also have access to these services.

If this method exists, it is called before the plugin is instantiated.

Each plugin has it's own separate service provider that holds references to the main application's services, it's dependencies' services and it's own
registerd services.

### Plugin Constructor
When a plugin is about to load, it is instantiated and the constructor is called. The plugin's constructor supports dependency injection.

Because the `Info` object, which contains the metainfo about the current plugin is not instantiated until after the plugin is loaded, the
metainfo instance is provided through the service provider. This means you can pass an instance of `IPluginMetaInfo` in the constructor's parameters
to get access to this while the plugin is loaded.

An example plugin with constructor:
```csharp
using System;
using EvoSC.Core.Plugins;

namespace EvoSC.Core.Modules.Info;

public class Info : EvoSCPlugin
{
    // IPluginMetaInfo is available form the plugin
    // ILogger is available from the main application's service provider
    public Info(IPluginMetaInfo info, ILogger<Info> logger)
    {
        logger.LogInformation("Hello from {Name}", info.Name);
    }
}
```

### Plugin Destructor
In the spirit of trying to minimize boilerplate and use C#'s features to set everything up. You can free up any long lasting resources when
a plugin unloads by declaring the class destructor.

### Enabling/Disabling plugins
*To be implemented / discussed*
