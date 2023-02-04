using System.Xml.Linq;
using System.Xml.XPath;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Models.Maps;
using EvoSC.Common.TextParsing;
using EvoSC.Common.TextParsing.ValueReaders;
using EvoSC.Common.Util.MatchSettings.Models;
using StringReader = EvoSC.Common.TextParsing.ValueReaders.StringReader;

namespace EvoSC.Common.Util.MatchSettings;

public static class MatchSettingsXmlParser
{
    private static ValueReaderManager _valueReader = new(
        new IntegerReader(),
        new BooleanReader(),
        new FloatReader(),
        new StringReader()
    );

    public static Task<IMatchSettings> ParseAsync(string xml) => ParseAsync(XDocument.Parse(xml));

    public static async Task<IMatchSettings> ParseAsync(XDocument document)
    {
        var playlistElement = document.Elements("playlist").First();

        var gameInfos = await ParseGameInfos(playlistElement);
        var filter = await ParseFilter(playlistElement);
        var scriptSettings = await ParseScriptSettings(playlistElement);
        var startIndex = await ParseStartIndex(playlistElement);
        var maps = await ParseMaps(playlistElement);

        return new MatchSettingsInfo
        {
            GameInfos = gameInfos,
            Filter = filter,
            ModeScriptSettings = scriptSettings,
            Maps = maps,
            StartIndex = startIndex
        };
    }

    private static async Task<MatchSettingsGameInfos> ParseGameInfos(XElement playlistElement)
    {
        var gameModeElement = playlistElement.XPathSelectElement("gameinfos/game_mode");
        var chatTimeElement = playlistElement.XPathSelectElement("gameinfos/chat_time");
        var finishTimeoutElement = playlistElement.XPathSelectElement("gameinfos/finishtimeout");
        var allWarmupDurationElement = playlistElement.XPathSelectElement("gameinfos/allwarmupduration");
        var disableRespawnElement = playlistElement.XPathSelectElement("gameinfos/disablerespawn");
        var forceShowAllOpponentsElement = playlistElement.XPathSelectElement("gameinfos/forceshowallopponents");
        var scriptNameElement = playlistElement.XPathSelectElement("gameinfos/script_name");

        return new MatchSettingsGameInfos
        {
            GameMode = await _valueReader.ConvertValueAsync<int>(ValueOrDefault(gameModeElement, "0")),
            ChatTime = await _valueReader.ConvertValueAsync<int>(ValueOrDefault(chatTimeElement, "10000")),
            FinishTimeout = await _valueReader.ConvertValueAsync<int>(ValueOrDefault(finishTimeoutElement, "1")),
            AllowWarmupDuration =
                await _valueReader.ConvertValueAsync<bool>(ValueOrDefault(allWarmupDurationElement, "0")),
            DisableRespawn = await _valueReader.ConvertValueAsync<bool>(ValueOrDefault(disableRespawnElement, "0")),
            ForceShowAllOpponents =
                await _valueReader.ConvertValueAsync<bool>(ValueOrDefault(forceShowAllOpponentsElement, "0")),
            ScriptName = ValueOrDefault(scriptNameElement, "")
        };
    }

    private static async Task<MatchSettingsFilter> ParseFilter(XElement playlistElement)
    {
        var isLanElement = playlistElement.XPathSelectElement("filter/is_lan");
        var isInternetElement = playlistElement.XPathSelectElement("filter/is_internet");
        var isSoloElement = playlistElement.XPathSelectElement("filter/is_solo");
        var isHotseatElement = playlistElement.XPathSelectElement("filter/is_hotseat");
        var sortIndexElement = playlistElement.XPathSelectElement("filter/sort_index");
        var randomMapOrderElement = playlistElement.XPathSelectElement("filter/random_map_order");

        return new MatchSettingsFilter
        {
            IsLan = await _valueReader.ConvertValueAsync<bool>(ValueOrDefault(isLanElement, "1")),
            IsInternet = await _valueReader.ConvertValueAsync<bool>(ValueOrDefault(isInternetElement, "1")),
            IsSolo = await _valueReader.ConvertValueAsync<bool>(ValueOrDefault(isSoloElement, "0")),
            IsHotseat = await _valueReader.ConvertValueAsync<bool>(ValueOrDefault(isHotseatElement, "0")),
            SortIndex = await _valueReader.ConvertValueAsync<int>(ValueOrDefault(sortIndexElement, "1000")),
            RandomMapOrder = await _valueReader.ConvertValueAsync<bool>(ValueOrDefault(randomMapOrderElement, "0"))
        };
    }

    private static async Task<int> ParseStartIndex(XElement playlistElement)
    {
        var startIndexElement = playlistElement.XPathSelectElement("startindex");
        return await _valueReader.ConvertValueAsync<int>(ValueOrDefault(startIndexElement, "0"));
    }

    private static async Task<Dictionary<string, ModeScriptSetting>> ParseScriptSettings(XElement playlistElement)
    {
        var settingElements = playlistElement.XPathSelectElements("script_settings/setting");

        var settings = new Dictionary<string, ModeScriptSetting>();

        foreach (var settingElement in settingElements)
        {
            var valueString = settingElement.Attribute("value")?.Value ?? ThrowAttributeError("value");
            
            if (valueString.Equals(string.Empty, StringComparison.Ordinal))
            {
                // ignore settings are empty, which means default value anyways
                continue;
            }
            
            var name = settingElement.Attribute("name")?.Value ?? ThrowAttributeError("name");
            var typeString = settingElement.Attribute("type")?.Value ?? ThrowAttributeError("type");
            var type = MatchSettingsMapper.ToType(typeString);

            var value = await _valueReader.ConvertValueAsync(type, valueString);

            settings[name] = new ModeScriptSetting {Value = value, Description = "", Type = type};
        }

        return settings;
    }

    private static Task<List<IMap>> ParseMaps(XElement playlistElement)
    {
        var mapElements = playlistElement.XPathSelectElements("map");

        var maps = new List<IMap>();

        foreach (var mapElement in mapElements)
        {
            var fileElement = mapElement.XPathSelectElement("file");
            var identElement = mapElement.XPathSelectElement("ident");

            var map = new Map
            {
                FilePath =
                    fileElement?.Value ?? throw new InvalidOperationException("Map does not contain a file."),
                Uid = ValueOrDefault(identElement, "")
            };

            maps.Add(map);
        }

        return Task.FromResult(maps);
    }

    private static string ThrowAttributeError(string attribute) =>
        throw new InvalidOperationException($"Missing attribute '{attribute}' in script setting.");

    private static string ValueOrDefault(XElement? element, string defaultValue)
    {
        if (element?.Value == null)
        {
            return defaultValue;
        }

        if (element.Value.Equals(string.Empty, StringComparison.Ordinal))
        {
            return defaultValue;
        }

        return element.Value;
    }
}
