using Claw.Commands;
using Claw.Documents;
using Claw.Validation;
using System;
using System.Globalization;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// Represents a binding of a command to a button.
    /// </summary>
    public class Band : NodeParser
    {
        private const string COMMAND_UUID_ATTRIBUTE = "command";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            COMMAND_UUID_ATTRIBUTE,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = { };

        internal override string[] RequiredAttributes
        {
            get { return REQUIRED_ATTRIBUTES; }
        }

        internal override string[] OptionalAttributes
        {
            get { return OPTIONAL_ATTRIBUTES; }
        }

        internal override string[] RequiredChildNodes
        {
            get { return REQUIRED_CHILD_NODES; }
        }

        internal override string[] OptionalChildNodes
        {
            get { return OPTIONAL_CHILD_NODES; }
        }
        
        internal override TagUsage TagUsageType
		{
			get { return TagUsage.Required; }
		}

        #endregion

        private uint identifier;
        private Guid commandUuid;
        /// <summary>
        /// The UUID of the associated command.
        /// </summary>
        internal Guid CommandUuid
        {
            get { return commandUuid; }
        }

        /// <summary>
        /// Creates a new band for the given command.
        /// </summary>
        /// <param name="command">The command to bind to.</param>
        public Band(Command command)
            : base()
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            identifier = 1;
            commandUuid = command.Uuid;
        }

        /// <summary>
        /// Creates a new Band.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "band" node.</param>
        internal Band(NodeValidator validator, Node node)
            : base(validator, node)
        {
            if (!string.IsNullOrEmpty(node.Tag))
            {
                identifier = uint.Parse(node.Tag, CultureInfo.InvariantCulture);
            }
            if (node.Attributes.ContainsKey(COMMAND_UUID_ATTRIBUTE))
            {
                commandUuid = new Guid(node.Attributes[COMMAND_UUID_ATTRIBUTE]);
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        internal Node CreateNode()
        {
            var node = new Node(BandList.BAND_CHILD_NODE.ToLowerInvariant());
            node.Tag = identifier.ToString(CultureInfo.InvariantCulture);
            if (commandUuid != null && commandUuid != Guid.Empty)
            {
                node.Attributes.Add(COMMAND_UUID_ATTRIBUTE, commandUuid.ToString());
            }
            return node;
        }
    }
}
