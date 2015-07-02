using System;
using System.Globalization;

namespace Claw.Commands
{
    /// <summary>
    /// Specifies the type of usage of an action block.
    /// </summary>
    public enum ActionBlockUsageType
    {
        /// <summary>
        /// The default type, specifies that all actions will be performed on button pressed.
        /// </summary>
        Press,

        /// <summary>
        /// ???
        /// </summary>
        Repeat,

        /// <summary>
        /// Specifies that all actions will be performed on button release.
        /// </summary>
        Release,
    }

    /// <summary>
    /// Helper class for ActionBlockUsageType.
    /// </summary>
    public static class ActionBlockUsageTypeHelper
    {
        /// <summary>
        /// Tries to parse the ActionBlockUsageType from the given string. Throws an ArgumentException if the parsing fails.
        /// </summary>
        /// <param name="type">The string to parse from.</param>
        /// <returns>The parsed ActionBlockUsageType.</returns>
        public static ActionBlockUsageType TryParse(string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            switch (type.ToUpperInvariant())
            {
                case "PRESS":
                    return ActionBlockUsageType.Press;

                case "REPEAT":
                    return ActionBlockUsageType.Repeat;

                case "RELEASE":
                    return ActionBlockUsageType.Release;

                default:
                    throw new ArgumentOutOfRangeException("Could not parse \"" + type + "\" to ActionBlockType.");
            }
        }

        /// <summary>
        /// Converts the given type to a string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "This method is required to bring strings to lower case as it is used to output the data into the profile files which require lower case strings.")]
        internal static string ToString(ActionBlockUsageType type)
        {
            return type.ToString("G").ToLowerInvariant();
        }
    }
}
