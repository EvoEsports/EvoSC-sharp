# ChatCommand Module
Manage commands that can be written in the chat.

## Example
### Injection (for PluginSystemBase)
```c#
[ChatCommand("test")]
void OnCommand(EntityPlayer player, int value)
{

}
```

### Manager
```c#
ChatCommandManager.Add("/test", OnCommand, arguments: new[]
{
    new Argument(typeof(int))
});

void OnCommand(PlayerEntity player, Span<Entity> args)
{
    var value = args[0].Get<int>();
}
```

The result of ChatCommandManager.Add() need to be added in the disposables of the caller.   

### Raw (entity based)
```c#
Entity entity = World.CreateEntity();
entity.Set(new IsChatCommand());
entity.Set(new CommandPath("/test"));
entity.Set(new CommandDescription(""));
entity.Set(new CommandInvoke((player, args) => {
    var value = args[0].Get<int>();
}); 
entity.Set(new ChatCommandArguments()
{
    new Argument(typeof(int))
});
```

The created entity need to be added in the disposables of the caller.