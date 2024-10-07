using System.ComponentModel;
using Config.Net;
using EvoSC.Modules.Attributes;

namespace EvoSC.Modules.EvoEsports.ToornamentModule.Settings
{
    [Settings]
    public interface IToornamentSettings
    {
        [Option, Description("The API key for the Toornament API.")]
        public string ApiKey { get; set; }

        [Option, Description("The client id for the Toornament API.")]
        public string ClientId { get; set; }

        [Option, Description("The client secret for the Toornament API.")]
        public string ClientSecret { get; set; }

        [Option, Description("The Toornament ID for the Toornament API.")]
        public string ToornamentId { get; set; }

        [Option, Description("List of player's account IDs which are always whitelisted for matches.")]
        public string Whitelist { get; }

        [Option(DefaultValue = false), Description("Whether to start the match automatically when all players are ready.")]
        public bool AutomaticMatchStart { get; set; }

        [Option, Description("The MatchId assigned to this server.")]
        public string AssignedMatchId { get; set; }

        [Option, Description("Use the Discipline defined in Toornament. If false, give a json string in Discipline setting.")]
        public bool UseToornamentDiscipline { get; set; }

        [Option, Description("A json string containing the Discipline settings (match settings).")]
        public string Disciplines { get; set; }

        [Option, Description("A comma separated string containing the Map Ids used on TMX, example: 186972,186973,186974,186870,186980")]
        public string MapTmxIds { get; set; }

        [Option, Description("A comma separated string containing the Map Ids used on Nadeo servers, example: zS5d30EU7x6meq2eIM5Uhu4UbTg,VbW1c9rTPSwtZNHrlDE8FVPqIca,lKJQ8YrXza3XiEN1a1fPK3fl4wf,W6GjI5Nsr9MYdBBOBXIOd8JhZwj,YUTy7o9O0hDmWFNVQ4QuxaXzXD4")]
        public string MapUids { get; set; }

        [Option, Description("A comma separated string containing the Map Ids used on Nadeo servers, example: b0c2e735-15a9-43df-a772-f5f1f692fa29,e8233617-a929-4891-924d-6ba72fd546c7,c7ffcdfe-fd98-417b-80d9-b464927f76a1,ae1eb6cb-76ef-45ab-8149-07377e3e45bd,b653936e-1316-461a-b4cf-befa6630e839")]
        public string MapIds { get; set; }

        [Option, Description("A comma separated string containing the Map Machine names as defined on Toornament, example: blueprint,domino,fe4turing,karotte,schwaadlappe")]
        public string MapMachineNames { get; set; }

        [Option(DefaultValue = 2), Description("The Group Id where players will get assigned to. This should be the Default group.")]
        public int DefaultGroupId { get; set; }

        [Option(DefaultValue = false), Description("Enable very sensitive logging, like logging Authorization tokens")]
        public bool SensitiveLogging { get; set; }

        [Option(DefaultValue = false), Description("Enable experimental features")]
        public bool UseExperimentalFeatures { get; set; }

        [Option(DefaultValue = true), Description("Use the default gamemode as defined in Discipline. Default value is true")]
        public bool UseDefaultGameMode { get; set; }

        [Option, Description("The Gamemode to be used when UseDefaultGameMode setting is false.")]
        public string GameModes { get; set; }

        [Option, Description("Specifies the Discord Webhook endpoint to send matchinformation to")]
        public string WebhookUrl { get; set; }

        [Option, Description("A suffix that will be added to each message. E.g. can be used for pinging certain @Roles or @Persons")]
        public string MessageSuffix { get; set; }
    }
}
