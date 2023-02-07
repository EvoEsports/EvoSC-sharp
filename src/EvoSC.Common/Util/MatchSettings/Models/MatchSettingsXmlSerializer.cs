using System.Xml.Linq;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Util;

namespace EvoSC.Common.Util.MatchSettings.Models;

/// <summary>
/// Provides XML serialization of match settings.
/// </summary>
public class MatchSettingsXmlSerializer : IMatchSettings
{
    public MatchSettingsGameInfos? GameInfos { get; set; }
    public MatchSettingsFilter? Filter { get; set; }
    public Dictionary<string, ModeScriptSettingInfo>? ModeScriptSettings { get; set; }
    public List<IMap>? Maps { get; set; }
    public int StartIndex { get; set; }

    /// <summary>
    /// Convert the match settings to an XML document.
    /// </summary>
    /// <returns></returns>
    public XDocument ToXmlDocument()
    {
        var xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", null));

        var playlistElement = XmlPlaylist(xmlDocument);

        if (GameInfos != null)
        {
            XmlGameInfosNode(playlistElement);
        }

        if (Filter != null)
        {
            XmlFilterNode(playlistElement);
        }

        if (ModeScriptSettings != null)
        {
            XmlScriptSettingsNode(playlistElement);
        }
        
        XmlStartIndexNode(playlistElement);

        if (Maps != null)
        {
            XmlMapListNodes(playlistElement);
        }
        
        return xmlDocument;
    }

    /// <summary>
    /// Create the root node.
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    private XElement XmlPlaylist(XDocument doc)
    {
        var playlist = new XElement("playlist");
        doc.Add(playlist);

        return playlist;
    }

    /// <summary>
    /// Create the game infos node.
    /// </summary>
    /// <param name="playlistElement"></param>
    private void XmlGameInfosNode(XElement playlistElement)
    {
        var gameInfosElement = new XElement("gameinfos");
        playlistElement.Add(gameInfosElement);
        
        gameInfosElement.Add(new XElement("game_mode", GameInfos.GameMode));
        gameInfosElement.Add(new XElement("chat_time", GameInfos.ChatTime));
        gameInfosElement.Add(new XElement("finishtimeout", GameInfos.FinishTimeout));
        gameInfosElement.Add(new XElement("allwarmupduration", GameInfos.AllWarmupDuration));
        gameInfosElement.Add(new XElement("disablerespawn", GameInfos.DisableRespawn));
        gameInfosElement.Add(new XElement("forceshowallopponents", GameInfos.ForceShowAllOpponents));
        gameInfosElement.Add(new XElement("script_name", GameInfos.ScriptName));
    }
    
    /// <summary>
    /// Create the filter node.
    /// </summary>
    /// <param name="playlistElement"></param>
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
    
    /// <summary>
    /// Loop through mode script settings and create the setting node.
    /// </summary>
    /// <param name="playlistElement"></param>
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

    /// <summary>
    /// Create the start_index node.
    /// </summary>
    /// <param name="playlistElement"></param>
    private void XmlStartIndexNode(XElement playlistElement)
    {
        playlistElement.Add(new XElement("start_index", StartIndex.ToString()));
    }
    
    /// <summary>
    /// Create all map nodes for this match settings.
    /// </summary>
    /// <param name="playlistElement"></param>
    private void XmlMapListNodes(XElement playlistElement)
    {
        foreach (var map in Maps)
        {
            var mapElement = new XElement("map");
            mapElement.Add(new XElement("file", map.FilePath));

            if (!string.IsNullOrEmpty(map.Uid))
            {
                mapElement.Add(new XElement("ident", map.Uid));
            }
            
            playlistElement.Add(mapElement);
        }
    }
}
