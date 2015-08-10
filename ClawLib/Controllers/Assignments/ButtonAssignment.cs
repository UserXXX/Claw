using Claw.Controllers.Controls;
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
        
        internal const string BANDS_CHILD_NODE = "BANDS";
        
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

        #endregion
        
        private string name;
        private ButtonAssignmentRole role;

        private BandList bands;

        /// <summary>
        /// The bands of this assignment.
        /// </summary>
        public BandList Bands
        {
            get { return bands; }
        }

        /// <summary>
        /// The role of the button. Internal as unprogrammed buttons are not read at all and their data is nonsense, all
        /// assignments available from extern have ButtonAssignmentRole.Bands.
        /// </summary>
        internal ButtonAssignmentRole Role
        {
            get { return role; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        protected override string NodeName
        {
            get { return AssignmentList.BUTTON_CHILD_NODE.ToLowerInvariant(); }
        }

        /// <summary>
        /// Creates a new button assignment for the given control.
        /// </summary>
        /// <param name="control">The control this assignment will assign to.</param>
        public ButtonAssignment(Control control)
            : base(control)
        {
            role = ButtonAssignmentRole.Bands;
            bands = new BandList();
        }

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

            if (role == ButtonAssignmentRole.Unprogrammed)
            {
                bands = new BandList();
            }
            else
            {
                foreach (var child in node.Children)
                {
                    if (child.Name.ToUpperInvariant() == BANDS_CHILD_NODE)
                    {
                        bands = new BandList(validator, child);
                    }
                }
            }
        }

        internal override void FillNode(Node node)
        {
            if (name != null)
            {
                node.Attributes.Add(NAME_ATTRIBUTE, name);
            }
            node.Attributes.Add(ROLE_ATTRIBUTE, ButtonAssignmentRoleHelper.ToString(role));
            if (bands != null)
            {
                node.Children.AddLast(bands.CreateNodes());
            }
        }
    }
}
