namespace AetharNet.Moonbow.Experimental.Utilities
{
    public readonly struct FormatInfo
    {
        public readonly string Command;
        public readonly string Argument;

        public FormatInfo(string command, string argument)
        {
            Command = command;
            Argument = argument;
        }

        public override string ToString()
        {
            return $"^{Command}:{Argument};";
        }
    }
}