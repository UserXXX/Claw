using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;

namespace Claw.Commands
{
    /// <summary>
    /// Represents an action command. This can be a macro or normal key strike.
    /// </summary>
    public class ActionCommand : Command
    {
        internal const string ACTION_BLOCK_CHILD_NODE = "actionblock";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            NameAttribute,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = {
            IconAttribute,
        };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
            ACTION_BLOCK_CHILD_NODE,
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

        #endregion

        private LinkedList<ActionBlock> actionBlocks = new LinkedList<ActionBlock>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        protected override string NodeName
        {
            get { return CommandList.ACTION_COMMAND_CHILD_NODE.ToLowerInvariant(); }
        }

        /// <summary>
        /// Creates a new ActionCommand.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "actioncommand" node.</param>
        internal ActionCommand(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                if (child.Name.ToUpperInvariant() == "ACTIONBLOCK")
                {
                    actionBlocks.AddLast(new ActionBlock(validator, child));
                }
            }
        }

        internal override void FillNode(Node node)
        {
            foreach (ActionBlock block in actionBlocks)
            {
                node.Children.AddLast(block.CreateNodes());
            }
        }
    }
}
