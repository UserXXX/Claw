using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// A list of assignments.
    /// </summary>
    public class AssignmentList : NodeListParser<Assignment>
    {
        internal const string MOUSE_POINTER_CHILD_NODE = "MOUSEPOINTER";
        internal const string BUTTON_CHILD_NODE = "BUTTON";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = { };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
        	MOUSE_POINTER_CHILD_NODE,
        	BUTTON_CHILD_NODE,
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
        /// Creates a new AssignmentList from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "assignments" node.</param>
        internal AssignmentList(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                switch (child.Name.ToUpperInvariant())
                {
                    case MOUSE_POINTER_CHILD_NODE:
                        Add(new MousePointerAssignment(validator, child));
                        break;

                    case BUTTON_CHILD_NODE:
                        Add(new ButtonAssignment(validator, child));
                        break;
                }
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification="Profile files need lowercase strings.")]
        internal Node CreateNodes()
        {
            var node = new Node(Shift.ASSIGNMENTS_CHILD_NODE.ToLowerInvariant());
            foreach (Assignment assignment in this)
            {
                node.Children.AddLast(assignment.CreateNodes());
            }
            return node;
        }
    }
}
