using Claw.Controllers.Assignments;
using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Claw.Controllers
{
    /// <summary>
    /// Represents the "modes" a device can have.
    /// </summary>
    public class Shift : NodeParser
    {
        private const string NAME_ATTRIBUTE = "name";
        private const string FALLBACK_ATTRIBUTE = "fallback";
        
        internal const string SELECTIONSET_CHILD_NODE = "selectionset";
        internal const string ASSIGNMENTS_CHILD_NODE = "assignments";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            NAME_ATTRIBUTE,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = {
        	FALLBACK_ATTRIBUTE,
        };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
        	SELECTIONSET_CHILD_NODE,
        	ASSIGNMENTS_CHILD_NODE,
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
			get { return TagUsage.Required; }
		}

        #endregion
        
        private Guid uuid;
        /// <summary>
        /// Specifies the fallback shift.
        /// </summary>
        private Guid fallback;
        private string name;

        /// <summary>
        /// "selectionset" node. This is a readonly hardware configuration. The given data is only relevant for drivers.
        /// This node is optional.
        /// </summary>
        private Node selectionset;

        private AssignmentList assignments;

        /// <summary>
        /// Creates a new Shift from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "shift" node.</param>
        internal Shift(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            if (!string.IsNullOrEmpty(node.Tag))
            {
                uuid = new Guid(node.Tag);
            }
            if (node.Attributes.ContainsKey(NAME_ATTRIBUTE))
            {
                name = node.Attributes[NAME_ATTRIBUTE];
            }
            if (node.Attributes.ContainsKey(FALLBACK_ATTRIBUTE))
            {
                fallback = new Guid(node.Attributes[FALLBACK_ATTRIBUTE]);
            }

            foreach (var child in node.Children)
            {
                switch (child.Name.ToLower())
                {
                    case SELECTIONSET_CHILD_NODE:
                        selectionset = child;
                        break;

                    case ASSIGNMENTS_CHILD_NODE:
                        assignments = new AssignmentList(validator, child);
                        break;
                }
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        internal Node CreateNodes()
        {
            var node = new Node(ShiftList.SHIFT_CHILD_NODE);
            if (uuid != null && uuid != Guid.Empty)
            {
                node.Tag = uuid.ToString();
            }
            if (fallback != null && fallback != Guid.Empty)
            {
                node.Attributes.Add(FALLBACK_ATTRIBUTE, fallback.ToString());
            }
            if (name != null)
            {
                node.Attributes.Add(NAME_ATTRIBUTE, name);
            }
            if (selectionset != null)
            {
                node.Children.AddLast(selectionset);
            }
            if (assignments != null)
            {
                node.Children.AddLast(assignments.CreateNodes());
            }
            return node;
        }
    }
}
