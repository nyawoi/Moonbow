using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    /// <summary>
    /// Utility class to format chat messages, as well as remove formatting and retrieve formatting data.
    /// </summary>
    public static class ChatFormat
    {
        /// <summary>
        /// Regular expression pattern to match formatting syntax.
        /// Explicit implementation of the pattern used in <c>/content/staxel/ui/js/chatFormat.js</c>.
        /// </summary>
        public static readonly Regex FormatPattern = new Regex(@"\^(c(?:olou?r)?|w(?:eight)?):(b(?:old)?|i(?:talics?)?|u(?:nderline)?|clear|pop|[0-F]{6}|[0-F]{3});", RegexOptions.IgnoreCase);

        /// <summary>
        /// Apply color formatting to message using integer.
        /// <example>
        /// <code>
        /// ChatFormat.Color("Long forgotten.", 0x10c);
        /// </code>
        /// </example>
        /// </summary>
        /// <remarks>Support for integers may be dropped on official release.</remarks>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Color(string message, int color)
        {
            var colorString = color.ToString("X").PadLeft(color <= 0xFFF ? 3 : 6, '0');
            return $"^c:{colorString};{message}^c:pop;";
        }
        /// <summary>
        /// Apply color formatting to message using hexadecimal string.
        /// <example>
        /// <code>
        /// ChatFormat.Color("You are", "fd6c9e");
        /// ChatFormat.Color("precious", "#ff5470");
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Color(string message, string color)
        {
            if (color[0] == '#')
            {
                color = color.Substring(1);
            }
            
            if (color.Length == 3)
            {
                color = $"{color[0]}{color[0]}{color[1]}{color[1]}{color[2]}{color[2]}";
            }
            
            return $"^c:{color};{message}^c:pop;";
        }

        /// <summary>
        /// TODO: Add documentation
        /// </summary>
        /// <remarks>Support for integers may be dropped on official release.</remarks>
        /// <param name="message"></param>
        /// <param name="initialColor"></param>
        /// <param name="finalColor"></param>
        /// <returns></returns>
        public static string Gradient(string message, int initialColor, int finalColor)
        {
            // There's probably a better way to do this, but I just want to get something out.
            // Allowing use of integers for color was a mistake.
            if (initialColor <= 0xFFF)
            {
                var colorString = initialColor.ToString("X").PadLeft(3, '0');
                initialColor = Convert.ToInt32($"{colorString[0]}{colorString[0]}{colorString[1]}{colorString[1]}{colorString[2]}{colorString[2]}");
            }

            if (finalColor <= 0xFFF)
            {
                var colorString = finalColor.ToString("X").PadLeft(3, '0');
                finalColor = Convert.ToInt32($"{colorString[0]}{colorString[0]}{colorString[1]}{colorString[1]}{colorString[2]}{colorString[2]}");
            }

            var initialColorR = initialColor >> 16;
            var initialColorG = (initialColor >> 8) & 255;
            var initialColorB = initialColor & 255;

            var finalColorR = finalColor >> 16;
            var finalColorG = (finalColor >> 8) & 255;
            var finalColorB = finalColor & 255;
            
            var totalSteps = message.Length;
            
            var stepR = (finalColorR - initialColorR) / totalSteps;
            var stepG = (finalColorG - initialColorG) / totalSteps;
            var stepB = (finalColorB - initialColorB) / totalSteps;

            var colors = new string[totalSteps];
            for (var i = 0; i < totalSteps; ++i)
            {
                var newR = initialColorR + (stepR * i);
                var newG = initialColorG + (stepG * i);
                var newB = initialColorB + (stepB * i);
                colors[i] = newR.ToString("X").PadLeft(2,'0') +
                            newG.ToString("X").PadLeft(2,'0') +
                            newB.ToString("X").PadLeft(2,'0');
            }
            
            var colorStack = colors.Reverse().Aggregate("", (current, color) => current + $"^c:{color};");
            var stackUsage = message.Aggregate(colorStack, (current, ch) => current + (ch + "^c:pop;"));

            return stackUsage;
        }
        /// <summary>
        /// TODO: Add documentation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="initialColor"></param>
        /// <param name="finalColor"></param>
        /// <returns></returns>
        public static string Gradient(string message, string initialColor, string finalColor)
        {
            var initialColorHex = Convert.ToInt32(initialColor[0] == '#' ? initialColor.Substring(1) : initialColor, 16);
            var finalColorHex = Convert.ToInt32(finalColor[0] == '#' ? finalColor.Substring(1) : finalColor, 16);

            return Gradient(message, initialColorHex, finalColorHex);
        }

        /// <summary>
        /// Apply bold text styling to message. If you wish to apply multiple styles, please use <c>Format()</c> instead. <see cref="Format(string,AetharNet.Moonbow.Experimental.Utilities.TextStyling)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Bold(string message)
        {
            return $"^w:b;{message}^w:clear;";
        }
        
        /// <summary>
        /// Apply italic text styling to message. If you wish to apply multiple styles, please use <c>Format()</c> instead. <see cref="Format(string,AetharNet.Moonbow.Experimental.Utilities.TextStyling)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Italic(string message)
        {
            return $"^w:i;{message}^:clear;";
        }
        
        /// <summary>
        /// Apply underline text styling to message. If you wish to apply multiple styles, please use <c>Format()</c> instead. <see cref="Format(string,AetharNet.Moonbow.Experimental.Utilities.TextStyling)"/>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Underline(string message)
        {
            return $"^w:u;{message}^:clear;";
        }
        
        /// <summary>
        /// Apply multiple text styling effects to message.
        /// <example>
        /// <code>
        /// ChatFormat.Format("Bold Italic", TextStyling.Bold | TextStyling.Italic);
        /// ChatFormat.Format("Bold Underline", TextStyling.Bold | TextStyling.Underline);
        /// ChatFormat.Format("Bold Italic Underline", TextStyling.Bold | TextStyling.Italic | TextStyling.Underline);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="styles"></param>
        /// <returns></returns>
        public static string Format(string message, TextStyling styles)
        {
            if (styles.HasFlag(TextStyling.Bold))
            {
                message = "^w:b;" + message;
            }

            if (styles.HasFlag(TextStyling.Italic))
            {
                message = "^w:i;" + message;
            }

            if (styles.HasFlag(TextStyling.Underline))
            {
                message = "^w:u;" + message;
            }

            return message + "^w:clear;";
        }
        /// <summary>
        /// TODO: Add documentation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="styles"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Format(string message, TextStyling styles, int color)
        {
            return Color(Format(message, styles), color);
        }
        /// <summary>
        /// TODO: Add documentation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="styles"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Format(string message, TextStyling styles, string color)
        {
            return Color(Format(message, styles), color);
        }

        /// <summary>
        /// TODO: Add documentation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string RemoveFormatting(string message)
        {
            return FormatPattern.Replace(message, "");
        }

        /// <summary>
        /// TODO: Add documentation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static List<FormatInfo> ParseFormatting(string message)
        {
            return (from Match match in FormatPattern.Matches(message) select new FormatInfo(match.Groups[0].Value, match.Groups[1].Value)).ToList();
        }
    }
}