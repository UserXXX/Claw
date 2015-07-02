using System;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// Enumeration for button roles.
    /// </summary>
    public enum ButtonAssignmentRole
    {
        /// <summary>
        /// The button is unprogrammed.
        /// </summary>
        Unprogrammed,

        /// <summary>
        /// The button uses the macro/key strike specified through its bands (=binds?).
        /// </summary>
        Bands,
    }

    /// <summary>
    /// Helper class for ButtonAssignmentRole enumeration.
    /// </summary>
    public static class ButtonAssignmentRoleHelper
    {
        /// <summary>
        /// Tries to parse the assignment role from the given string. If parsing fails an ArgumentException is thrown.
        /// </summary>
        /// <param name="role">The string to parse from.</param>
        /// <returns>The parsed ButtonAssignmentRole.</returns>
        public static ButtonAssignmentRole TryParse(string role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            switch (role.ToUpperInvariant())
            {
                case "UNPROGRAMMED":
                    return ButtonAssignmentRole.Unprogrammed;

                case "BANDS":
                    return ButtonAssignmentRole.Bands;
                
                default:
                    throw new ArgumentOutOfRangeException("Could not parse ButtonAssigmentRole from \"" + role + "\".");
            }
        }

        /// <summary>
        /// Converts the given role to a string.
        /// </summary>
        /// <param name="role">The role to convert.</param>
        /// <returns>The created string.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "This method is required to bring strings to lower case as it is used to output the data into the profile files which require lower case strings.")]
        internal static string ToString(ButtonAssignmentRole role)
        {
            return role.ToString("G").ToLowerInvariant();
        }
    }
}
