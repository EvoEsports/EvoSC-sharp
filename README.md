# EvoSC# Prototype

## Projects
- [Controller](Prototype.EvoSC/README.md)
- [Plugin.Samples](Plugin.Samples/README.md)

## First Run

For a single-file output check [Publishing](#publishing)

1. Build the project:
`dotnet build`
2. Create the server config file at:
`<Binary>/Config/server.json`
```json
{
  "host": "127.0.0.1",
  "port": 5000,
  
  "login": "SuperAdmin",
  "password": "SuperAdmin"
}
```
3. Run the project:
`dotnet <Binary>/Prototype.EvoSC.dll run`

## Run with samples

1. Follow steps from [the First Run](#first-run)
2. Create an autoload config file at:
`<Binary>/Config/modules.json`
3. Include the Plugin.Samples module:
```json
{
  "load": [
    "Plugin.Samples"
  ]
}
```
4. Run the project:
   `dotnet <Binary>/Prototype.EvoSC.dll run`

## Publishing
Used for generating a single-file executable.

`dotnet publish -r [TARGET] --self-contained true`
- The **TARGET** can be:
  - linux-x64
  - win-x64
- If **self-contained** is set to:
  - False, then the .NET Runtime will not be included in the app (it will need to be installed in the OS).
  - True, then users who download the controller will not need to install the .NET Runtime since it will be already included in the app.

The file will be in `Prototype.EvoSC/bin/Debug/net5.0/[TARGET]/publish`

Then run it `./Prototype.EvoSC run` or `./Prototype.EvoSC.exe run`