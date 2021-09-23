namespace EvoSC.Utility.Commands
{
    /// <summary>
    /// The path of a command
    /// </summary>
    public readonly struct CommandPath
    {
        public readonly string Value;

        public CommandPath(string value)
        {
            Value = value;
        }
    }
}
