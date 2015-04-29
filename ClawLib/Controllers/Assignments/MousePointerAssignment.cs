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
    	private const string MOUSE_AXIS_CHILD_NODE = "mouseaxis";
    	
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
        
		internal override TagUsage TagUsageType
		{
			get { return TagUsage.NotAllowed; }
		}

        #endregion
        
        private LinkedList<MouseAxisAssignment> mouseAxes = new LinkedList<MouseAxisAssignment>();

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
                if (child.Name.ToLower() == MOUSE_AXIS_CHILD_NODE)
            	{
                	mouseAxes.AddLast(new MouseAxisAssignment(validator, child));
                }
            }
        }
    }
}
