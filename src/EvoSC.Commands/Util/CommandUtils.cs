using System.Text.RegularExpressions;

namespace EvoSC.Commands.Util;

public static class CommandUtils
{
    public static void ValidateCommandName(string name)
    {
        if (!Regex.IsMatch(name, "[\\w\\d]+"))
        {
            throw new ArgumentException("Command name must be alphanumeric", "name");
        }
    }
}
