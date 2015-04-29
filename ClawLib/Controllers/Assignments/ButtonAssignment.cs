using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// An assignment to a button.
    /// </summary>
    public class ButtonAssignment : Assignment
    {
        private const string NAME_ATTRIBUTE = "name";
        private const string ROLE_ATTRIBUTE = "role";
        
        private const string BANDS_CHILD_NODE = "bands";
        
		#region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
        	ROLE_ATTRIBUTE,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = {
			NAME_ATTRIBUTE,
		};
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
        	BANDS_CHILD_NODE,
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
        
        private string name;
        private ButtonAssignmentRole role;

        private BandList bands;

        /// <summary>
        /// Creates a new ButtonAssignment.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "button" node.</param>
        internal ButtonAssignment(NodeValidator validator, Node node)
            : base(validator, node)
        {
            if (node.Attributes.ContainsKey(NAME_ATTRIBUTE))
            {
                name = node.Attributes[NAME_ATTRIBUTE];
            }
            if (node.Attributes.ContainsKey(ROLE_ATTRIBUTE))
            {
                role = ButtonAssignmentRoleHelper.TryParse(node.Attributes[ROLE_ATTRIBUTE]);
            }

            foreach (var child in node.Children)
            {
                if (child.Name.ToLower() == BANDS_CHILD_NODE)
                {
                    bands = new BandList(validator, child);
                }
            }
        }
    }
}
