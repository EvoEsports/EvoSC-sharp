# EvoSC-Sharp-Toornament
This module is used to connect a server to the Toornament website. Toornament is a website to organize competitions for many games, including Trackmania. With this module it's possible to setup a server with specific match data provided by Toornament and at the end of a match the scores are relayed back to the Toornament website. This way a bracket and winner can be determined for a competition.


## Setup Environment Variables

The module has the following Environment Variables:

|Name|Docker|Type|Required|Description|
|----|------|-----|--------|-----------|
|ApiKey|EVOSC_MODULE_TOORNAMENTMODULE_APIKEY|string|Yes|The Api key to connect with the Toornament Api.|
|ClientId|EVOSC_MODULE_TOORNAMENTMODULE_CLIENTID|string|Yes|The ClientId to connect with the Toornament Api.|
|ClientSecret|EVOSC_MODULE_TOORNAMENTMODULE_CLIENTSECRET|string|Yes|The Client secret to connect with the Toornament Api.|
|ToornamentId|EVOSC_MODULE_TOORNAMENTMODULE_TOORNAMENTID|string|No|The Toornament Id to be pre-selected for this event.|
|Whitelist|EVOSC_MODULE_TOORNAMENTMODULE_WHITELIST|string|No|The Comma-separated string of Account IDs (NOT LoginId) of players who should be whitelisted on the server. |
|AutomaticMatchStart|EVOSC_MODULE_TOORNAMENTMODULE_AUTOMATICMATCHSTART|bool|No|Flag to determine whether a Match should start automatically when all the players are ready. If false, the admin has to use `/toornament_startmatch` to start the match manually.|
|AssignedMatchId|EVOSC_MODULE_TOORNAMENTMODULE_ASSIGNEDMATCHID|string|No|NOT IMPLEMENTED TO BE SET AND USED FROM ENVIRONMENT VARIABLE. This is currently used internally to keep track of the Match this server is configured for.|
|UseToornamentDiscipline|EVOSC_MODULE_TOORNAMENTMODULE_USETOORNAMENTDISCIPLINE|bool|Yes|UNTESTED! Flag to determine whether to use the Discipline defined on the Toornament website or not. The module has only been tested with the value `False` and the `Disciplines` variable set.|
|Disciplines|EVOSC_MODULE_TOORNAMENTMODULE_DISCIPLINES|string|Yes*|Required if we don't use the Discipline configured on the Toornament website. Description, see below.| 
|MapTmxIds|EVOSC_MODULE_TOORNAMENTMODULE_MAPTMXIDS|string|Yes*|A comma separated string containing the Map Ids used on TMX. Required when `MapUids` and `MapIds` are empty.|
|MapUids|EVOSC_MODULE_TOORNAMENTMODULE_MAPUIDS|string|Yes*|A comma separated string containing the Map Uids used on this server. It is used to determine whether the maps have already been provided or have to be downloaded. Required when `MapTmxIds` or `MapIds` are empty.|
|MapIds|EVOSC_MODULE_TOORNAMENTMODULE_MAPIDS|string|Yes*|A comma separated string containing the Map Ids used on Nadeo servers. Required when `MapTmxIds` or `MapUids` are empty.|
|MapMachineNames|EVOSC_MODULE_TOORNAMENTMODULE_MAPMACHINENAMES|Yes|A comma separated string containing the MachineNames of the maps as defined on Toornament. The Toornament API doesn't provide an endpoint for this, so we have to provide it|
|SensitiveLogging|EVOSC_MODULE_TOORNAMENTMODULE_SENSITIVELOGGING|bool|No|Enable very sensitive logging, like logging Authorization tokens|
|UseExperimentalFeatures|EVOSC_MODULE_TOORNAMENTMODULE_USEEXPERIMENTALFEATURES|bool|No|Flag to indicate whether to use experimental features or not. This is currently applying a whitelist to players on the server or players joining the server. If they are not on the whitelist, they will get kicked from the server. Default value is `False`.|
|UseDefaultGameMode|EVOSC_MODULE_TOORNAMENTMODULE_USEDEFAULTGAMEMODE|bool|No|Flag to indicate whether to use a Nadeo gamemode or a custom gamemode. Default value is `true`.|
|GameMode|EVOSC_MODULE_TOORNAMENTMODULE_GAMEMODE|string|Yes*|Required when using a custom gamemode script. This indicates the path to where the script can be found.|
|WebhookUrl|EVOSC_MODULE_TOORNAMENTMODULE_WEBHOOKURL|string|No|String indicating a Discord webhook url where maporder will be posted to.|
|MessageSuffix|EVOSC_MODULE_TOORNAMENTMODULE_MESSAGESUFFIX|string|No|Extra information that can be added to the Discord message. For example pinging certain @roles or @users.|

## Disciplines

A Tournament can have different matchsettings to be used during different stages of the tournament. In Toornament these settings are called Disciplines. We can define multiple Disciplines as an Json array in the `Disciplines` environment variable. 

A single Discipline has the following structure:
```Json
{
    "game_mode": "rounds", 
    "group_number": 1, 
    "plugins": 
    {
        "S_UseAutoReady": false
    }, 
    "round_number": 1, 
    "scripts": 
    {
        "S_DelayBeforeNextMap": 2000, 
        "S_FinishTimeout": 3, 
        "S_MapsPerMatch": 2, 
        "S_NbOfWinners": 1, 
        "S_PointsLimit": -1, 
        "S_PointsRepartition": "n-1", 
        "S_RespawnBehaviour": 0, 
        "S_RoundsPerMap": 1, 
        "S_UseTieBreak": false, 
        "S_WarmUpDuration": 5, 
        "S_WarmUpNb": 1
    }, 
    "stage_number": 1, 
    "tracks_shuffle": true
}
```

### GameMode
The game mode to be used during this stage of the Tournament. Available options are:
- rounds
- cup
- time_attack
- knockout
- laps
- team

### StageNumber, GroupNumber, RoundNumber
The specific combination of StageNumber, GroupNumber and RoundNumber (as provided by Toornament) where this Discipline will be used. Since it's possible to provide an array of Disciplines, we can define different Disciplines for multiple stages / groups / rounds at once.

### Plugins
Unused and untested at the moment. This is configured in Toornament and can be used with the Nadeo competition tool. For each value, see https://doc.trackmania.com/club/competition-tool/plugin-settings/ for the specific explanations.

### TracksShuffle
Flag to indicate whether the maps should be shuffled or not. TODO: Shuffling is currently the only option to determine a different map order. Players have requested a Pick (and Ban) phase to determine the map order. This is currently not available.

### Scripts
Specific script settings to be used for this match as defined in a Rulebook. For each value, see https://wiki.trackmania.io/en/dedicated-server/Usage/OfficialGameModesSettings for the specific explanations.

S_PointsRepartition is different though. This value can contain one of two options:
- The default Comma-separated list of numbers to determine the point repartition from first to last (e.g. 10,6,4,3,2,1)
- The string `"n-1"`. This will look at the number of Assigned players in a match (For Toornament this is `Match.Opponents.Length()`) and generate the Comma-separated list of numbers (e.g. Opponents=60 -> 60,59,58...3,2,1).



## Chat commands

### /toornament_setup
This command will show a setup window where an organizer can select the specific Toornament from a list, followed by the specific Stage and Match to be played. When the match is selected, the server will setup the defined gamemode and gamesettings from either the `Disciplines` environment variable or from the discipline defined on the Toornament website.

Video:

https://github.com/user-attachments/assets/cb115c67-112a-40e9-9ce0-98f41099d6d1

### /toornament_startmatch
Start the match for the configured match on this server. Can only be called after `/toornament_setup`.

### /setpoints \<playername> \<number>
Change the points of the given playername to the provided number. 

### /servername \<name>
Sets the name of the server to the provided name.
