using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Claw.Commands
{
    /// <summary>
    /// A list of commands.
    /// </summary>
    public class CommandList : NodeListParser<Command>
    {
        internal const string ACTION_COMMAND_CHILD_NODE = "ACTIONCOMMAND";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = { };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
        	ACTION_COMMAND_CHILD_NODE,
        };

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
            get { return TagUsage.NotAllowed; }
        }

        #endregion
        
        /// <summary>
        /// Default constructor. Creates an empty list.
        /// </summary>
        public CommandList()
            : base()
        { }

        /// <summary>
        /// Creates a new CommandList from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "commands" node.</param>
        internal CommandList(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                if (child.Name.ToUpperInvariant() == ACTION_COMMAND_CHILD_NODE)
                {
                    Add(new ActionCommand(validator, child));
                }
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        internal Node CreateNodes()
        {
            var node = new Node(MadCatzProfile.COMMANDS_CHILD_NODE.ToLowerInvariant());
            foreach (Command command in this)
            {
                node.Children.AddLast(command.CreateNodes());
            }
            return node;
        }
    }
}
