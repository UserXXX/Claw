﻿using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;

namespace Claw.Commands
{
    /// <summary>
    /// A block of actions.
    /// </summary>
    public class ActionBlock : NodeParser
    {
        private const string TYPE_ATTRIBUTE = "type";

        private const string ACTION_CHILD_NODE = "action";

        #region Check Stuff

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

        private LinkedList<Action> actions = new LinkedList<Action>();

        /// <summary>
        /// Creates a new ActionBlock.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "actionblock" node.</param>
        internal ActionBlock(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            if (!string.IsNullOrEmpty(node.Tag))
                blockType = ActionBlockTypeHelper.TryParse(node.Tag);

            if (node.Attributes.ContainsKey(TYPE_ATTRIBUTE))
                type = ActionBlockUsageTypeHelper.TryParse(node.Attributes[TYPE_ATTRIBUTE]);

            foreach (var child in node.Children)
            {
                if (child.Name.ToLower() == ACTION_CHILD_NODE)
                    actions.AddLast(new Action(validator, child));
            }
        }
    }
}
