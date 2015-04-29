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
        private const string MOUSE_POINTER_CHILD_NODE = "mousepointer";
        private const string BUTTON_CHILD_NODE = "button";

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
            get { return TagUsage.Required; }
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
                switch (child.Name.ToLower())
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
    }
}
