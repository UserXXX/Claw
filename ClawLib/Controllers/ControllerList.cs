using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;

namespace Claw.Controllers
{
	/// <summary>
	/// List of controllers.
	/// </summary>
	public class ControllerList : NodeListParser<Controller>
    {
        internal const string CONTROLLER_CHILD_NODE = "controller";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = { };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = {
            CONTROLLER_CHILD_NODE,
        };
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

        internal override TagUsage TagUsageType
        {
            get { return TagUsage.NotAllowed; }
        }

        #endregion

        /// <summary>
        /// Default constructor. Creates an empty list.
        /// </summary>
        public ControllerList()
            : base()
        { }

        /// <summary>
        /// Creates a new ControllerList.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "controllers" node.</param>
        internal ControllerList(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                if (child.Name == CONTROLLER_CHILD_NODE)
                {
                    Add(new Controller(validator, child));
                }
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        internal Node CreateNodes()
        {
            var node = new Node(MadCatzProfile.CONTROLLERS_CHILD_NODE.ToLowerInvariant());
            foreach (Controller controller in this)
            {
                node.Children.AddLast(controller.CreateNodes());
            }
            return node;
        }
    }
}
