using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Claw.Controllers.Controls
{
    /// <summary>
    /// A list for controls.
    /// </summary>
    public class ControlList : NodeListParser<Control>
    {
        private const string MOUSE_POINTER_CHILD_NODE = "mousepointer";
        private const string MOUSE_AXIS_CHILD_NODE = "mouseaxis";
        private const string BUTTON_CHILD_NODE = "button";
        private const string SLIDER_CHILD_NODE = "slider";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = { };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
        	MOUSE_POINTER_CHILD_NODE,
        	MOUSE_AXIS_CHILD_NODE,
        	BUTTON_CHILD_NODE,
            SLIDER_CHILD_NODE,
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
        /// Creates a new ControlList from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "controls" node.</param>
        internal ControlList(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                switch (child.Name.ToLower())
                {
                    case MOUSE_POINTER_CHILD_NODE:
                        Add(new MousePointerControl(validator, child));
                        break;

                    case MOUSE_AXIS_CHILD_NODE:
                        Add(new MouseAxisControl(validator, child));
                        break;

                    case BUTTON_CHILD_NODE:
                        Add(new ButtonControl(validator, child));
                        break;

                    case SLIDER_CHILD_NODE:
                        Add(new SliderControl(validator, child));
                        break;
                }
            }
        }
    }
}
