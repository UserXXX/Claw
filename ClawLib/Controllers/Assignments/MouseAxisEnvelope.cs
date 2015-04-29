using System;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// Specifies the type of assignment for a MouseAxis.
    /// </summary>
    public enum MouseAxisEnvelope
    {
        /// <summary>
        /// Envelope for sensitivity values.
        /// </summary>
        Sensitivity,
    }

    /// <summary>
    /// Helper class for MouseAxisEnvelope enumeration.
    /// </summary>
    public static class MouseAxisEnvelopeHelper
    {
        /// <summary>
        /// Tries to parse an envelope. If parsing fails an ArgumentException is thrown.
        /// </summary>
        /// <param name="envelope">The string to parse from.</param>
        /// <returns>The parsed MouseAxisEnvelope.</returns>
        public static MouseAxisEnvelope TryParse(string envelope)
        {
            if (envelope.ToLower() == "sensitivity")
                return MouseAxisEnvelope.Sensitivity;
            else
                throw new ArgumentException("Could not parse MouseAxisEnvelope from \"" + envelope + "\".");
        }
    }
}
