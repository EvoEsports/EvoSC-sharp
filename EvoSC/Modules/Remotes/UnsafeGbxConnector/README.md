# Provide a performance focused GBX remote.

### Remarks
This GBX remote/connector isn't the default one, and the module will be put into another project (aka it will never be the default one)
___
## Comparing to GBXRemote.Net
### Pros
- Extremely fast serialization and deserialization (see benchmarks)
- Extremely low GC and memory usage 
- Low CPU usage
- Automatically batches calls into a 'multicall' instruction
### Cons
- Need to manually serialize and deserialize methods and structs.
- While very performant, it's unsafe, so problems will be hard to detect.
- Not everything is implemented yet.

## So when to use it?
This remote should be used in contexts where a controller crash is accepted and performance is required:
- A large casual server with a lot of players

It shouldn't be used if:
- You're hosting a competition/tournament (except if it's a test tournament to test this remote)
- You need the controller to never crash.

Note: there shouldn't be any crash, but it's not a hard guarantee.

## Enabling it
In the controller settings, add this in `config.json` :  
```json
{
  "remote": "EvoSC.Modules.Remotes.UnsafeGbxConnector"
}
```  

(enabling it may be different in further commits, it's just a short thing I made)