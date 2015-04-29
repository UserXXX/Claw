using System;

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
            switch (type.ToLower())
            {
                case "press":
                    return ActionBlockUsageType.Press;

                case "repeat":
                    return ActionBlockUsageType.Repeat;

                case "release":
                    return ActionBlockUsageType.Release;

                default:
                    throw new ArgumentException("Could not parse \"" + type + "\" to ActionBlockType.");
            }
        }
    }
}
