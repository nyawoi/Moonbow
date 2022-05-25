using System;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    /// <summary>
    /// Enumeration to easily apply text styling to messages using <c>ChatFormat.Format()</c>.
    /// </summary>
    [Flags]
    public enum TextStyling
    {
        Bold      = 1 << 0,
        Italic    = 1 << 1,
        Underline = 1 << 2
    }
}