# Flow
This is the standard flow when you do `dotnet run -- run`:
```c#
Main(argument = "run")
    // load default systems
    HostModule()
        GatherModules()
    
    // load CLI module
    BaseModule("Modules/CLI/Module.cs")
        ExecuteCommandSystem(argument) // argument which is run
              InteractiveRunSystem.OnRunCommand()
                LoadModule("Core/EvoSCEntryModule.cs")
                ---

---
"Core/EvoSCEntryModule.cs"
    LoadModule("Modules/ServerConnection/Module.cs")        
    LoadModule("Modules/ChatCommand/Module.cs")
        
    // Each loadable modules have a 'module.config' file.
    // If the field 'autoLoad' is set to true, 
    // then it will get automatically loaded
    LoadModulesMarkedWithAutoLoadInConfig()
    
---
"Modules/ServerConnection/Module.cs"
    LoadServerConfig("Config/server.json")
    CreateServerDomain()
    
    OnServerDomain
        ConnectToServer()
        CreateEvents()
        ManageEventLoop()
    
---
"Modules/ChatCommand/Module.cs"
    OnServerDomain
        OnPlayerChat
            ChatCommandExecuteSystem()
        
Loadable Modules
---

// More information in the README of the module/plugin
"Plugin.Samples"
```

**Remarks**     
This is pseudo code, so you don't really load a module from its path,    
instead you load it from its namespace + class name.    
- `LoadModule("Prototype.EvoSC.ChatCommand", "ChatCommand")`
- `LoadModule("Plugin.Samples")`