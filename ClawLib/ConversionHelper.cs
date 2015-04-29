using System;
using System.Globalization;

namespace Claw
{
    /// <summary>
    /// Helper class for conversion of data from the Node object structure.
    /// </summary>
    public static class ConversionHelper
    {
        /// <summary>
        /// Parses an uint form the given text. If parsing fails an exception is thrown.
        /// </summary>
        /// <param name="text">The text to parse from.</param>
        /// <returns>The parsed uint.</returns>
        public static uint ParseHexUint(string text)
        {
            if (text.StartsWith("0x", StringComparison.CurrentCulture))
                text = text.Substring(2);

            return uint.Parse(text, NumberStyles.HexNumber);
        }
    }
}
