# Remarks
[Safety](#safety)  
[Performance](#performance)  
[ECS](#ecs)  

___
## Safety
[Task Scheduler](#constrainedtaskscheduler)  
[Update Order](#update-order)  
[Explicit Flow](#explicit-flow)  
[Memory Leak](#memory-leaks)  

### ConstrainedTaskScheduler
A custom task scheduler which force all user tasks to be run on the domain thread.  
This make async **100% fool-proof**. (thread safety issues begone!)  
It's still possible to await a method from another thread in a ConstrainedTaskScheduler.  
It's also possible to create a threaded call on a ConstrainedTaskScheduler:  
`Task.Run(() => YourMethodAsync(args));`
### Update Order
When a system subscribe to an IEventLoopSubscriber they can pass a custom order, and are able to either update after or before another system.
### Explicit Flow
With the framework based on ECS, we always have an explicit flow:
- All actions that change any public members is public by itself.
- All data that can be controlled only contains public members.
- Side effects of a data/entity can't exist until a system does more than one thing.
- Modules define the organization of the controller/plugins.
- Systems must only do one thing.
- Systems first define which data they require, and then modify it later in an update loop.
### Memory Leaks
- All systems in the controller are guaranteed to not have memory leaks. (until it's a third party package)

___
## Performance
[Reflection](#reflection)  
[Memory and GC](#memory-and-gc)  
[GbxRemoteClient (GbxRemote.Net)](#gbxremoteclient)  

### Reflection
The whole code does not contains any reflection, except:
- **[PluginSystemBase](Core/Plugins/PluginSystemBase.cs)**
  - Required for dependencies
  - Required for custom injectors (such as chat commands)
- *(implicit)* **[GbxStructExtensions.GetXmlRpcType()](Core/Remote/Structs/IGbxStruct.cs#L33)**
  - Contains a call to ResponseMessage.GetResponseData() which call Activator.CreateInstance
    - (This can easily be fixed with a simple switch case based on the name)
### Memory and GC
There is no boxing for value types done (except for outside packages).  
Classes are only created on initialization.  
Pooling in hot paths for arrays and lists is obligated.  
Entities are used for passing composed arguments.  
### GbxRemoteClient
The controller and plugins don't directly use the functions of GbxRemoteClient.  
Instead it use IGbxRemote which encapsulate the functions.  
This is particularly useful for unit tests, but also for performance:
- One can test performance improvement on a custom IGbxRemote, check if everything work alright compared to the base one, and then implement it into the base one.
- One can create a fake GbxRemote for unit testing (to see if your plugin work, etc...)

In this case, the currently used IGbxRemote is [ForceThreadedGbxRemote](Core/Remote/ForceThreadedGbxRemote.cs):
- It force all calls to be done on another thread.
- Calls that either require a struct argument or struct return are manually deserialized.
### Others
- ServerDomain warn you when there is a lag spike (delta > **50ms**).
- When ServerDomain is idle, its delta time (aka how much time it worked in one update loop) should be under **0.01ms**

___
## ECS
The prototype is mostly ECS (Entity-Component-System) based. 
  - For people who know the MVC pattern (Model-View-Controller), then it's almost the same.
  - For people who don't, then just imagine a class (Entity) that contains multiple fields (Component) that is processed by another class (System)
  - It could also be understood as a relational database (The key being the Entity, and data being linked to the Entity, and program controlling/querying the database being the System)