namespace EvoSC.Utility.Commands
{
    /// <summary>
    /// The description of a command
    /// </summary>
    public readonly struct CommandDescription
    {
        public readonly string Value;

        public CommandDescription(string value)
        {
            Value = value;
        }
    }
}
