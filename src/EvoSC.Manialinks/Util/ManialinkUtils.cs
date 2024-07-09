using System.Text;
using ManiaTemplates.Lib;

namespace EvoSC.Manialinks.Util;

public static class ManialinkUtils
{
    public static string CreateHideManialink(string name)
    {
        var sb = new StringBuilder()
            .Append("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\" ?>\n")
            .Append("<manialink version=\"3\" id=\"")
            .Append(ManialinkNameUtils.KeyToId(name))
            .Append("\">\n")
            .Append("</manialink>\n");

        return sb.ToString();
    }
}
