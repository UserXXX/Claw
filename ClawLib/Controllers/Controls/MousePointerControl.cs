using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;

namespace Claw.Controllers.Controls
{
    /// <summary>
    /// A control that points to the screen, consists of two axes.
    /// </summary>
    public class MousePointerControl : Control
    {
    	private const string MOUSE_AXIS_CHILD_NODE = "mouseaxis";
    	
    	#region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            NAME_ATTRIBUTE,
        };
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
    	
        private LinkedList<MouseAxisControl> axes = new LinkedList<MouseAxisControl>();

        protected override string NodeName
        {
            get { return ControlList.MOUSE_POINTER_CHILD_NODE; }
        }

        /// <summary>
        /// Creates a new MousePointer from a node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "mousepointer" node.</param>
        internal MousePointerControl(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                if (child.Name == MOUSE_AXIS_CHILD_NODE)
                {
                    axes.AddLast(new MouseAxisControl(validator, child));
                }
            }
        }

        internal override void FillNode(Node node)
        {
            foreach (MouseAxisControl mouseAxis in axes)
            {
                node.Children.AddLast(mouseAxis.CreateNodes());
            }
        }
    }
}
