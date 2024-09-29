# Server Synchronization Module
This module exposes an API for synchronizing states between multiple servers.

It works by leveraging the NATS message broker and JetStream for reliability and replay capabilities.

## Installation
### Setting up NATS
First step is to set up a NATS stream.

In addition, for the subjects to be accepted, we need to define them within the stream.
The subjects used by the module is `<StreamName>.ChatMessages`, `<StreamName>.PlayerState` and `<StreamName>.MapFinished`.

For example:
```bash
nats stream add --defaults --subjects "MyServerSyncStream.ChatMessages,MyServerSyncStream.PlayerState,MyServerSyncStream.MapFinished" MyServerSyncStream
```

You can also edit these subjects later on with the `nats stream edit` command.

### Installing the module
To install the module in EvoSC#, first build the project and open the output directory of the module project.

Under the directory `<EvoSC binary root>/modules` create a new directory called `ServerSync` and copy the following files from the module project's output directory to that directory:
- `info.toml`
- `ServerSync.dll`
- `NATS.Client.dll`

You can now start EvoSC# and it should generate the module config in the database.

### Configuring the module
The module provides some configuration for connecting to the the NATS server.

| Name                                                    | Description                                                                                                                                         |
|---------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------|
| ServerSyncModule.NatsSettings.Host                      |  The host address for the NATS server. Default is `127.0.0.1`                                                                                       |
| ServerSyncModule.NatsSettings.Port                      |  The port for the NATS server. Default is `4222`                                                                                                    |
| ServerSyncModule.NatsSettings.MessageGroup              |  The message group name for the stream. This is typically the stream name itself. Default is `ServerSync`                                           |
| ServerSyncModule.NatsSettings.PlayerStatesStartSequence | You typically don't need to edit this value. But essentially, when the module starts up, all messages from this start sequence ID will be replayed. |

## Usage
Using the module is done through the `ISyncService` service, as well as subscribing to events raised by the module.

Chat messages and map finishes are automatically published to all the servers and it is generally not needed to call these methods from the `ISyncService` service.

But to publish a player's state, one can call the `PublishPlayerStateAsync` method. It provides several overloads for your convenience.

### `ISyncService` methods:

| Method                                                                                                                                           | Description                                                              |
|--------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------|
| `PublishChatMessageAsync(IPlayer player, string message);`                                                                                       | Publish a chat message to all connected servers.                         |
| `PublishPlayerStateAsync(IPlayer player, long position, IEnumerable<long> scores, IEnumerable<long> checkpointScores, IEnumerable<long> times);` | Publish a player state to all connected servers.                         |
| `PublishPlayerStateAsync(IPlayer player, IEnumerable<long> scores)`                                                                              | Publish the scores of a player to all connected servers.                 |
| `PublishPlayerStateAsync(IPlayer player, long position)`                                                                                         | Publish the position of a player to all servers.                         |
| `PublishPlayerStateAsync(IPlayer player, long position)`                                                                                         | Publish the position of a player to all servers.                         |
| `PublishPlayerStateAsync(IPlayer player, long position, IEnumerable<long> scores)`                                                               | Publish the position and scores of a player to all servers.              |
| `PublishPlayerStateAsync(IPlayer player, long position, IEnumerable<long> scores, IEnumerable<long> checkpointScores)`                           | Publish the position, scores and checkpoints of a player to all servers. |
| `PublishMapFinished()`                                                                                                                           | Publish a map finished/ended event to all servers.                       |
|                                                                                                                                                  |                                                                          |

### Subscriptions

| Name                | Args                         | Description                                              |
|---------------------|------------------------------|----------------------------------------------------------|
| `PlayerStateUpdate` | `PlayerStateUpdateEventArgs` | When a player state was updated from one of the servers. |
| `ChatMessage`       | `ChatStateMessageEventArgs`  | When a chat message was sent from one of the servers.    |
| `MapFinished`       | `MapFinishedStateEventArgs`  | When a server has their map finished.                    |
