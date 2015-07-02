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
        public static uint ParseHexValue(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (text.StartsWith("0x", StringComparison.CurrentCulture))
            {
                text = text.Substring(2);
            }

            return uint.Parse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts an uint to a hex string.
        /// </summary>
        /// <param name="value">uint to convert.</param>
        /// <returns>The created string with leading "0x".</returns>
        internal static string FormatHexUInt(uint value)
        {
            string hex = value.ToString("X", CultureInfo.InvariantCulture);
            // *.pr0 files use 8 hex signs in each uint.
            while (hex.Length < 8)
            {
                hex = "0" + hex;
            }
            return "0x" + hex;
        }
    }
}
