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
        
        private const string SELECTIONSET_CHILD_NODE = "selectionset";
        private const string ASSIGNMENTS_CHILD_NODE = "assignments";
        
        private const string MOUSE_POINTER_CHILD_NODE = "mousepointer";
        private const string BUTTON_CHILD_NODE = "button";

        #region Check Stuff

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

        private LinkedList<Assignment> assignments = new LinkedList<Assignment>();

        /// <summary>
        /// Creates a new Shift from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "shift" node.</param>
        internal Shift(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            uuid = new Guid(node.Tag);
            name = node.Attributes[NAME_ATTRIBUTE];

            if (node.Attributes.ContainsKey(FALLBACK_ATTRIBUTE))
                fallback = new Guid(node.Attributes[FALLBACK_ATTRIBUTE]);

            foreach (var child in node.Children)
            {
                switch (child.Name.ToLower())
                {
                    case SELECTIONSET_CHILD_NODE:
                        selectionset = child;
                        break;

                    case ASSIGNMENTS_CHILD_NODE:
                        LoadAssignments(validator, child);
                        break;
                }
            }
        }

        /// <summary>
        /// Loads the assignments.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "assignments" node.</param>
        private void LoadAssignments(NodeValidator validator, Node node)
        {
            foreach (var child in node.Children)
            {
                switch (child.Name.ToLower())
                {
                    case MOUSE_POINTER_CHILD_NODE:
                        assignments.AddLast(new MousePointerAssignment(validator, child));
                        break;

                    case BUTTON_CHILD_NODE:
                        assignments.AddLast(new ButtonAssignment(validator, child));
                        break;

                    default:
                        Trace.WriteLine("Unknown node type in \"assignments\" node: " + child.Name);
                        break;
                }
            }
        }
    }
}
