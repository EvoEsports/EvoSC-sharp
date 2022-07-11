# Chat Commands

This is a framework that allows you to declare chat commands.

## Basic example

In this example, we declare a command /ping that replies "Pong!" back to the player.

Start off by declaring a class and inheriting `ChatCommandGroup`, this base class defines a group of command and
provides the command context when instantiated. Each method must have a Task return type and annotated by the `Command`
attribute. You define the name of the command in the `Command` attribute along with a description. It's important to
note that command names can only be alphanumeric.

```cs
public class ExampleChatCommands : ChatCommandGroup
{
    [Command("ping", "Ping the server!")]
    public Task Ping() =>
        Context.Message.ReplyAsync("Pong!");
}
```

## Command Arguments

Arguments are defined in the parameters of the command method. You can specify the type you need and the values will be
automatically converted to the correct types when a user calls the command. If a user types an invalid argument for a
type, an error will be returned to the user.

You can also provide optional arguments by setting a default value to the method parameter.

An example of a command that adds two numbers together:

```cs
public class ExampleChatCommands : ChatCommandGroup
{
    [Command("sum", "Ping the server!")]
    public Task Mycommand(int a, int b) =>
        Context.Message.ReplyAsync($"The sum of the two numbers are {a + b}");
}
```

### Argument description

It's possible to add a description to the arguments by annotating the parameters with the `Description` attribute:

```cs
public class ExampleChatCommands : ChatCommandGroup
{
    [Command("echo", "Have the server say something!")]
    public Task Mycommand([Description("Message to send to everyone.")] string message) =>
        Context.Client.ChatSendServerMessageAsync("[Server] " + message);
}
```

It's optional, but it can help the end users understand your command by providing a description.

## Command Groups / Sub Commands

You can define whether a command lies within a group or not. When a command resides within a command group, all calls to
the command must be prefixed by the group name. For example if a group is named *mx* and the command is called *addmap*
the command must be called like this: */mx addmap*

To create a command group, either annotate the class or the individual methods with the `CommandGroup` attribute.

Here is an example:

```cs
[CommandGroup("mx", "Add maps to the server.")]
public class ExampleChatCommands : ChatCommandGroup
{
    [Command("addmap", "Add a map from ManiaExchange.")]
    public Task Mycommand([Description("MX ID of the map.")] int mxId) {
        // ...
    }
        
    [Command("addpack", "Add a mappack from ManiaExchange")]
    public Task Mycommand([Description("MX ID of the mappack.")] int packId) {
        // ...
    }
}
```

or by individual methods:

```cs
public class ExampleChatCommands : ChatCommandGroup
{
    [CommandGroup("mx")]
    [Command("addmap", "Add a map from ManiaExchange.")]
    public Task Mycommand([Description("MX ID of the map.")] int mxId) {
        // ...
    }
    
    [CommandGroup("mx")]
    [Command("addpack", "Add a mappack from ManiaExchange")]
    public Task Mycommand([Description("MX ID of the mappack.")] int packId) {
        // ...
    }
}
```

In the last example, all commands defined with the same group name will be grouped together.

## Permissions

There are two ways to define restriction to commands. Etiher you can restrict every command in an entire class, or you
can restrict individual commands.

For example, to restrict every command within a class:

```cs
[Permission("admin", "Admin only commands")]
public class ExampleChatCommands : ChatCommandGroup
{
    // ..
}
```

Or by individual commands:

```cs
public class ExampleChatCommands : ChatCommandGroup
{
    [Permission("admin", "Admin only commands")]
    [CommandGroup("mx")]
    [Command("addmap", "Add a map from ManiaExchange.")]
    public Task Mycommand([Description("MX ID of the map.")] int mxId) {
        // ...
    }
    
    [Permission("admin")]
    [CommandGroup("mx")]
    [Command("addpack", "Add a mappack from ManiaExchange")]
    public Task Mycommand([Description("MX ID of the mappack.")] int packId) {
        // ...
    }
}
```

Note that the description of the first defined permission will take effect.
