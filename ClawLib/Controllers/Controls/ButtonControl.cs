using Claw.Documents;
using Claw.Validation;
using System;

namespace Claw.Controllers.Controls
{
    /// <summary>
    /// Class representing a pressable hardware button.
    /// </summary>
    public class ButtonControl : Control
    {
        private const string LATCHABLE_ATTRIBUTE = "latchable";

        #region Check Stuff

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            NAME_ATTRIBUTE,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = {
        	LATCHABLE_ATTRIBUTE,
        };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
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
        
        /// <summary>
        /// Attribute with default value false. Whether the button can be latched, meaning that a press will virtually hold it until the next press.
        /// </summary>
        private bool latchable = true;

        /// <summary>
        /// Creates a new button from a given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "button" node.</param>
        internal ButtonControl(NodeValidator validator, Node node)
            : base(validator, node)
        {
            if (node.Attributes.ContainsKey(LATCHABLE_ATTRIBUTE))
                latchable = bool.Parse(node.Attributes[LATCHABLE_ATTRIBUTE]);
        }
    }
}
