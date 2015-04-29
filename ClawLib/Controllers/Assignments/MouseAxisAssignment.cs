using Claw.Documents;
using Claw.Validation;
using System;
using System.Diagnostics;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// Represents an assignment to a mouse axis.
    /// </summary>
    public class MouseAxisAssignment : Assignment
    {
        private const string ENVELOPE_ATTRIBUTE = "envelope";

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
        
		internal override TagUsage TagUsageType
		{
			get { return TagUsage.NotAllowed; }
		}

        #endregion
        
        private MouseAxisEnvelope envelope;

        private int value;

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
                envelopeName = node.Attributes[ENVELOPE_ATTRIBUTE].ToLower();
                envelope = MouseAxisEnvelopeHelper.TryParse(envelopeName);
            }

            if (!string.IsNullOrEmpty(envelopeName))
            {
                foreach (var child in node.Children)
                {
                    if (child.Name.ToLower() == envelopeName)
                    {
                        value = int.Parse(child.Tag);
                    }
                }
            }
        }
    }
}
