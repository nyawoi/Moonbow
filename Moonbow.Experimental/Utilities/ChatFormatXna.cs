using System.Linq;
using Color = Microsoft.Xna.Framework.Color;

namespace AetharNet.Moonbow.Experimental.Utilities
{
    public static class ChatFormatXna
    {
        /// <summary>
        /// Apply color formatting to message using XNA Color struct.
        /// <example>
        /// <code>
        /// ChatFormat.Color("EXPLOSION", Color.Crimson);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Color(string message, Color color)
        {
            var colorString = color.R.ToString("X").PadLeft(2,'0') +
                              color.G.ToString("X").PadLeft(2,'0') +
                              color.B.ToString("X").PadLeft(2,'0');
            return $"^c:{colorString};{message}^c:pop;";
        }

        /// <summary>
        /// TODO: Add documentation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="initialColor"></param>
        /// <param name="finalColor"></param>
        /// <returns></returns>
        public static string Gradient(string message, Color initialColor, Color finalColor)
        {
            var totalSteps = message.Length;
            
            var stepR = (finalColor.R - initialColor.R) / totalSteps;
            var stepG = (finalColor.G - initialColor.G) / totalSteps;
            var stepB = (finalColor.B - initialColor.B) / totalSteps;

            var colors = new string[totalSteps];
            for (var i = 0; i < totalSteps; ++i)
            {
                var newR = initialColor.R + (stepR * i);
                var newG = initialColor.G + (stepG * i);
                var newB = initialColor.B + (stepB * i);
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
        /// <param name="styles"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string Format(string message, TextStyling styles, Color color)
        {
            return Color(ChatFormat.Format(message, styles), color);
        }
    }
}