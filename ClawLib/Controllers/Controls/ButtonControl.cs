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
        private const string LATCHED_ATTRIBUTE = "latched";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            NameAttribute,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = {
        	LATCHABLE_ATTRIBUTE,
            LATCHED_ATTRIBUTE,
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
        /// Attribute with default value true. Whether the button can be latched, meaning that a press will virtually hold it until the next press.
        /// </summary>
        private bool latchable = true;
        /// <summary>
        /// Attribute with default value false. Whether the button is a latched button.
        /// </summary>
        private bool latched = false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        protected override string NodeName
        {
            get { return ControlList.BUTTON_CHILD_NODE.ToLowerInvariant(); }
        }

        /// <summary>
        /// Creates a new button from a given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "button" node.</param>
        internal ButtonControl(NodeValidator validator, Node node)
            : base(validator, node)
        {
            if (node.Attributes.ContainsKey(LATCHABLE_ATTRIBUTE))
            {
                latchable = bool.Parse(node.Attributes[LATCHABLE_ATTRIBUTE]);
            }
            if (node.Attributes.ContainsKey(LATCHED_ATTRIBUTE))
            {
                latched = bool.Parse(node.Attributes[LATCHED_ATTRIBUTE]);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification="The profile files require lower case strings.")]
        internal override void FillNode(Node node)
        {
            if (!latchable)
            {
                node.Attributes.Add(LATCHABLE_ATTRIBUTE, latchable.ToString().ToLowerInvariant());
            }
            if (latched)
            {
                node.Attributes.Add(LATCHED_ATTRIBUTE, latchable.ToString().ToLowerInvariant());
            }
        }
    }
}
