using Claw.Documents;
using Claw.Validation;
using System;

namespace Claw.Controllers.Controls
{
    /// <summary>
    /// A control representing a single axis.
    /// </summary>
    public class MouseAxisControl : Control
    {
    	#region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            NameAttribute,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        protected override string NodeName
        {
            get { return ControlList.MOUSE_AXIS_CHILD_NODE.ToLowerInvariant(); }
        }

        /// <summary>
        /// Creates a new MouseAxis from a node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "mouseaxis" node.</param>
        internal MouseAxisControl(NodeValidator validator, Node node)
            : base(validator, node)
        { }

        internal override void FillNode(Node node) { }
    }
}
