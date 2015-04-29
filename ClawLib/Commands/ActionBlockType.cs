using System;

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
            if (type.ToLower() == "macro")
                return ActionBlockType.Macro;
            else
                throw new ArgumentException("Could not parse \"" + type + "\" to ActionBlockType.");
        }
    }
}
