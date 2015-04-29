using System;

namespace Claw.Commands
{
    /// <summary>
    /// Enumeration for values, which are button states.
    /// </summary>
    public enum ActionValue : uint
    {
        /// <summary>
        /// The button is released.
        /// </summary>
        Released = 0x00000000,

        /// <summary>
        /// The button is pressed.
        /// </summary>
        Pressed = 0x00000001,
    }
}
