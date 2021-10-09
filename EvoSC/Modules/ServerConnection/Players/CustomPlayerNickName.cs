namespace EvoSC.Modules.ServerConnection
{
    public readonly struct CustomPlayerNickName
    {
        public readonly string Value;

        public CustomPlayerNickName(string value)
        {
            Value = value;
        }

        public static implicit operator string(CustomPlayerNickName component) => component.Value;
    }
}
