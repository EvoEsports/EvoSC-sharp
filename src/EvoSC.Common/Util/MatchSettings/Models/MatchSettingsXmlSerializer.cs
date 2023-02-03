using System.Xml.Linq;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Util.MatchSettings.Models;

public class MatchSettingsXmlSerializer : IMatchSettings
{
    public MatchSettingsGameInfos GameInfos { get; set; }
    public MatchSettingsFilter Filter { get; set; }
    public Dictionary<string, ModeScriptSetting> ModeScriptSettings { get; set; }
    public List<IMap> Maps { get; set; }
    public int StartIndex { get; set; }
    
    public XDocument ToXmlDocument()
    {
        var xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", null));

        var playlistElement = XmlPlaylist(xmlDocument);
        
        XmlGameInfosNode(playlistElement);
        XmlFilterNode(playlistElement);
        XmlScriptSettingsNode(playlistElement);
        XmlStartIndexNode(playlistElement);
        XmlMapListNodes(playlistElement);
        
        return xmlDocument;
    }

    private XElement XmlPlaylist(XDocument doc)
    {
        var playlist = new XElement("playlist");
        doc.Add(playlist);

        return playlist;
    }

    private void XmlGameInfosNode(XElement playlistElement)
    {
        var gameInfosElement = new XElement("gameinfos");
        playlistElement.Add(gameInfosElement);
        
        gameInfosElement.Add(new XElement("game_mode", GameInfos.GameMode));
        gameInfosElement.Add(new XElement("chat_time", GameInfos.ChatTime));
        gameInfosElement.Add(new XElement("finishtimeout", GameInfos.FinishTimeout));
        gameInfosElement.Add(new XElement("allowwarmupduration", GameInfos.AllowWarmupDuration));
        gameInfosElement.Add(new XElement("disablerespawn", GameInfos.DisableRespawn));
        gameInfosElement.Add(new XElement("forceshowallopponents", GameInfos.ForceShowAllOpponents));
        gameInfosElement.Add(new XElement("script_name", GameInfos.ScriptName));
    }
    
    private void XmlFilterNode(XElement playlistElement)
    {
        var filterElement = new XElement("filter");
        playlistElement.Add(filterElement);
        
        filterElement.Add(new XElement("is_lan", Filter.IsLan));
        filterElement.Add(new XElement("is_internet", Filter.IsInternet));
        filterElement.Add(new XElement("is_solo", Filter.IsSolo));
        filterElement.Add(new XElement("is_hotseat", Filter.IsHotseat));
        filterElement.Add(new XElement("sort_index", Filter.SortIndex));
        filterElement.Add(new XElement("random_map_order", Filter.RandomMapOrder));
    }
    
    private void XmlScriptSettingsNode(XElement playlistElement)
    {
        var scriptSettingsElement = new XElement("script_settings");
        playlistElement.Add(scriptSettingsElement);

        foreach (var (key, setting) in ModeScriptSettings)
        {
            scriptSettingsElement.Add(new XElement(
                "setting",
                new XAttribute("name", key),
                new XAttribute("value", setting.Value?.ToString() ?? ""),
                new XAttribute("type", MatchSettingsMapper.ToTypeString(setting.Type))
            ));
        }
    }

    private void XmlStartIndexNode(XElement playlistElement)
    {
        playlistElement.Add(new XElement("start_index", StartIndex.ToString()));
    }
    
    private void XmlMapListNodes(XElement playlistElement)
    {
        foreach (var map in Maps)
        {
            playlistElement.Add(new XElement("map",
                new XElement("file", map.FilePath),
                new XElement("ident", map.Uid)
            ));
        }
    }
}
