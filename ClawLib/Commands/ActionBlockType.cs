using System;
using System.Globalization;

namespace Claw.Commands
{
    /// <summary>
    /// Specifies the type of an action block.
    /// </summary>
    public enum ActionBlockType
    {
        /// <summary>
        /// The default type, specifies that all keys will be pressed at the same time and immediately be released again.
        /// </summary>
        Default,

        /// <summary>
        /// The block is a macro. The given timing values will be used and keys will not be released automatically.
        /// </summary>
        Macro,
    }

    /// <summary>
    /// Helper class for ActionBlockType.
    /// </summary>
    public static class ActionBlockTypeHelper
    {
        /// <summary>
        /// Tries to parse the ActionBlockType from the given string. Throws an ArgumentException if the parsing fails.
        /// </summary>
        /// <param name="type">The string to parse from.</param>
        /// <returns></returns>
        public static ActionBlockType TryParse(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(type);
            }

            if (type.ToUpperInvariant() == "MACRO")
            {
                return ActionBlockType.Macro;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Could not parse \"" + type + "\" to ActionBlockType.");
            }
        }

        /// <summary>
        /// Converts the given type to a string.
        /// </summary>
        /// <param name="blockType">The type to convert.</param>
        /// <returns>The string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification="This method is required to bring strings to lower case as it is used to output the data into the profile files which require lower case strings.")]
        internal static string ToString(ActionBlockType blockType)
        {
            return blockType.ToString("G").ToLowerInvariant();
        }
    }
}
