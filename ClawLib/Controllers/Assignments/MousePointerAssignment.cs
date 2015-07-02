using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// Represents an assignment to a mousepointer control.
    /// </summary>
    public class MousePointerAssignment : Assignment
    {
    	internal const string MOUSE_AXIS_CHILD_NODE = "MOUSEAXIS";
    	
    	#region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = { };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = {
        	MOUSE_AXIS_CHILD_NODE,
        };
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

        #endregion
        
        private LinkedList<MouseAxisAssignment> mouseAxes = new LinkedList<MouseAxisAssignment>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        protected override string NodeName
        {
            get { return AssignmentList.MOUSE_POINTER_CHILD_NODE.ToLowerInvariant(); }
        }

        /// <summary>
        /// Creates a new MousePointerAssignment.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "mousepointer" node.</param>
        internal MousePointerAssignment(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                if (child.Name.ToUpperInvariant() == MOUSE_AXIS_CHILD_NODE)
            	{
                	mouseAxes.AddLast(new MouseAxisAssignment(validator, child));
                }
            }
        }

        internal override void FillNode(Node node)
        {
            foreach (MouseAxisAssignment mouseAxis in mouseAxes)
            {
                node.Children.AddLast(mouseAxis.CreateNodes());
            }
        }
    }
}
