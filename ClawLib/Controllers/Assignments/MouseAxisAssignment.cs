using Claw.Documents;
using Claw.Validation;
using System;
using System.Diagnostics;
using System.Globalization;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// Represents an assignment to a mouse axis.
    /// </summary>
    public class MouseAxisAssignment : Assignment
    {
        private const string ENVELOPE_ATTRIBUTE = "envelope";

        private const string SENSITIVITY_ENVELOPE = "sensitivity";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            ENVELOPE_ATTRIBUTE,
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
        
		internal override bool CheckNodes
		{
			get { return false; }
		}

        #endregion
        
        private MouseAxisEnvelope envelope;

        private int value;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        protected override string NodeName
        {
            get { return MousePointerAssignment.MOUSE_AXIS_CHILD_NODE.ToLowerInvariant(); }
        }

        /// <summary>
        /// Creates a new MouseAxisAssignment.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "mouseaxis" node.</param>
        internal MouseAxisAssignment(NodeValidator validator, Node node)
            : base(validator, node)
        {
            string envelopeName = null;
            if (node.Attributes.ContainsKey(ENVELOPE_ATTRIBUTE))
            {
                envelopeName = node.Attributes[ENVELOPE_ATTRIBUTE].ToUpperInvariant();
                envelope = MouseAxisEnvelopeHelper.TryParse(envelopeName);
            }

            if (!string.IsNullOrEmpty(envelopeName))
            {
                foreach (var child in node.Children)
                {
                    if (child.Name.ToUpperInvariant() == envelopeName)
                    {
                        value = int.Parse(child.Tag, CultureInfo.InvariantCulture);
                    }
                }
            }
        }

        internal override void FillNode(Node node)
        {
            if (envelope == MouseAxisEnvelope.Sensitivity)
            {
                node.Attributes.Add(ENVELOPE_ATTRIBUTE, SENSITIVITY_ENVELOPE);
                Node envelopeNode = new Node(SENSITIVITY_ENVELOPE);
                envelopeNode.Tag = value.ToString(CultureInfo.InvariantCulture);
                node.Children.AddLast(envelopeNode);
            }
        }
    }
}
