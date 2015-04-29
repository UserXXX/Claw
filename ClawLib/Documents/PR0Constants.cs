using System;

namespace Claw.Documents
{
    /// <summary>
    /// Container for constants used in the *.pr0 file format.
    /// </summary>
    internal static class PR0Constants
    {
        /// <summary>
        /// Opening character for nodes.
        /// </summary>
        internal const char OPEN_NODE_CHARCTER = '[';

        /// <summary>
        /// Closing character for nodes.
        /// </summary>
        internal const char CLOSE_NODE_CHARCTER = ']';

        /// <summary>
        /// Character for attribute value assignments.
        /// </summary>
        internal const char ASSIGN_CHARACTER = '=';

        /// <summary>
        /// Character for the opening of an enclosing statement for objects that contain whitespace characters.
        /// </summary>
        internal const char ENCLOSING_OPEN_CHARACTER = '\'';

        /// <summary>
        /// Character for the closing of an enclosing statement for objects that contain whitespace characters.
        /// </summary>
        internal const char ENCLOSING_CLOSE_CHARACTER = '\'';

        /// <summary>
        /// Character for the closing of a data section.
        /// </summary>
        internal const char TERMINATE_DATA_SECTION_CHARACTER = '>';
    }
}
