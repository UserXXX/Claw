using System;

namespace Claw.Controllers
{
    /// <summary>
    /// Represents groups of devices.
    /// </summary>
    public enum DeviceGroup
    {
        /// <summary>
        /// Group for mice.
        /// </summary>
        Mice,

        /// <summary>
        /// Group for keyboards.
        /// </summary>
        Keyboard,
    }

    /// <summary>
    /// Helper class with convienience methods for DeviceGroup.
    /// </summary>
    public static class DeviceGroupHelper
    {
        /// <summary>
        /// Tries to parse the device group from the given string.
        /// </summary>
        /// <param name="group">The group in string representation.</param>
        /// <returns>The parsed device group.</returns>
        public static DeviceGroup TryParse(string group)
        {
            if (group == "Mice")
                return DeviceGroup.Mice;
            else if (group == "Keyboard")
                return DeviceGroup.Keyboard;
            else
                throw new ArgumentException("Could not parse \"" + group + "\" to device group.");
        }
    }
}
