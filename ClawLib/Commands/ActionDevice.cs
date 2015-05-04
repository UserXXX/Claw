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
            switch (device.ToLower())
            {
                case "keyboard":
                    return ActionDevice.Keyboard;

                case "mouse":
                    return ActionDevice.Mouse;

                case "hotkey":
                    return ActionDevice.Hotkey;

                default:
                    throw new ArgumentException("Could not parse ActionDevice from \"" + device + "\".");
            }
        }

        /// <summary>
        /// Converts the given device to a string.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>The string.</returns>
        internal static string ToString(ActionDevice device)
        {
            return device.ToString("G").ToLower();
        }
    }
}
