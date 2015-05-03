using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;

namespace Claw.Controllers.Controls
{
    /// <summary>
    /// Represents a control with multiple states. This can be a button with multiple states (like R.A.T.9 mode switch button).
    /// </summary>
    public class SliderControl : Control
    {
    	private const string BUTTON_CHILD_NODE = "button";
    	
    	#region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            NAME_ATTRIBUTE,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = {
        	BUTTON_CHILD_NODE,
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
    	
        private LinkedList<ButtonControl> buttons = new LinkedList<ButtonControl>();

        protected override string NodeName
        {
            get { return ControlList.SLIDER_CHILD_NODE; }
        }

        /// <summary>
        /// Creates a new Slider.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "slider" node.</param>
        internal SliderControl(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                if (child.Name.ToLower() == BUTTON_CHILD_NODE)
                {
                    buttons.AddLast(new ButtonControl(validator, child));
                }
            }
        }

        internal override void FillNode(Node node)
        {
            foreach (ButtonControl button in buttons)
            {
                node.Children.AddLast(button.CreateNodes());
            }
        }
    }
}
