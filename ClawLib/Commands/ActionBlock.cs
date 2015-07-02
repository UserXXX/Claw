using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Claw.Commands
{
    /// <summary>
    /// A block of actions.
    /// </summary>
    public class ActionBlock : NodeListParser<Action>
    {
        private const string TYPE_ATTRIBUTE = "type";

        internal const string ACTION_CHILD_NODE = "ACTION";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = { };
        private static readonly string[] OPTIONAL_ATTRIBUTES = {
            TYPE_ATTRIBUTE,
        };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
            ACTION_CHILD_NODE,
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
			get { return TagUsage.Optional; }
		}

        #endregion

        private ActionBlockType blockType = ActionBlockType.Default;
        private ActionBlockUsageType type = ActionBlockUsageType.Press;

        /// <summary>
        /// Creates a new ActionBlock.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "actionblock" node.</param>
        internal ActionBlock(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            if (!string.IsNullOrEmpty(node.Tag))
            {
                blockType = ActionBlockTypeHelper.TryParse(node.Tag);
            }
            if (node.Attributes.ContainsKey(TYPE_ATTRIBUTE))
            {
                type = ActionBlockUsageTypeHelper.TryParse(node.Attributes[TYPE_ATTRIBUTE]);
            }

            foreach (var child in node.Children)
            {
                if (child.Name.ToUpperInvariant() == ACTION_CHILD_NODE)
                {
                    Add(new Action(validator, child));
                }
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        internal Node CreateNodes()
        {
            var node = new Node(ActionCommand.ACTION_BLOCK_CHILD_NODE);
            if (blockType != ActionBlockType.Default)
            {
                node.Tag = ActionBlockTypeHelper.ToString(blockType);
            }
            if (type != ActionBlockUsageType.Press)
            {
                node.Attributes.Add(TYPE_ATTRIBUTE, ActionBlockUsageTypeHelper.ToString(type));
            }
            foreach (Action action in this)
            {
                node.Children.AddLast(action.CreateNodes());
            }
            return node;
        }
    }
}
