using System;

namespace Claw.Commands
{
    /// <summary>
    /// Enumeration for values, which are button states.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification="The native profile values are uints, so they are used here instead of int.")]
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
