# Events
## Structs

### EventOnConnected
Created when the server has finished connecting.
```c#
struct EventOnConnected
{

}
```

### EventOnPlayerConnect    
Created when a player has joined the server.
- The player entity may not have its ServerPlayerInfo component set (you can either await the result or block to get the result)
```c#
struct EventOnPlayerConnect
{
    PlayerEntity Player;
    bool IsSpectator;
}
```

### EventOnPlayerDisconnect
Created when a player has left the server.
- The player entity may not have its ServerPlayerInfo component set (you can either await the result or block to get the result)
```c#
struct EventOnPlayerDisconnect
{
    PlayerEntity Player;
    string Reason;
}
```

### EventOnPlayerChat
Created when a player is chatting.
- The player entity may not have its ServerPlayerInfo component set (you can either await the result or block to get the result)
```c#
struct EventOnPlayerChat
{
    PlayerEntity Player;
    string Text;
}
```