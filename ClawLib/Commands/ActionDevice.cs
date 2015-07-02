using System;

namespace Claw.Commands
{
    /// <summary>
    /// Represents a device for an action.
    /// </summary>
    public enum ActionDevice
    { 
        /// <summary>
        /// The action is performed on the keyboard.
        /// </summary>
        Keyboard,

        /// <summary>
        /// The action is performed on the mouse.
        /// </summary>
        Mouse,

        /// <summary>
        /// ???
        /// </summary>
        Hotkey,
    }

    /// <summary>
    /// Helper class for ActionDevice enumeration.
    /// </summary>
    public static class ActionDeviceHelper
    {
        /// <summary>
        /// Tries to parse the ActionDevice. If parsing fails an ArgumentException is thrown.
        /// </summary>
        /// <param name="device">The string to parse from.</param>
        /// <returns>The parsed ActionDevice.</returns>
        public static ActionDevice TryParse(string device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }

            switch (device.ToUpperInvariant())
            {
                case "KEYBOARD":
                    return ActionDevice.Keyboard;

                case "MOUSE":
                    return ActionDevice.Mouse;

                case "HOTKEY":
                    return ActionDevice.Hotkey;

                default:
                    throw new ArgumentOutOfRangeException("Could not parse ActionDevice from \"" + device + "\".");
            }
        }

        /// <summary>
        /// Converts the given device to a string.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>The string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "This method is required to bring strings to lower case as it is used to output the data into the profile files which require lower case strings.")]
        internal static string ToString(ActionDevice device)
        {
            return device.ToString("G").ToLowerInvariant();
        }
    }
}
