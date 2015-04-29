using Claw.Documents;
using Claw.Validation;
using System;

namespace Claw.Controllers
{
    /// <summary>
    /// Member of a controller group, identifies a specific device.
    /// </summary>
    public class Member : NodeParser
    {
        private const string NAME_ATTRIBUTE = "name";
        private const string SHORT_NAME_ATTRIBUTE = "shortname";

        #region Check Stuff

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            NAME_ATTRIBUTE,
            SHORT_NAME_ATTRIBUTE,
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
        
		internal override TagUsage TagUsageType
		{
			get { return TagUsage.Required; }
		}

        #endregion
        
        private Guid uuid;
        private string name;
        private string shortName;

        /// <summary>
        /// Creates a new Member from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The node to parse from.</param>
        internal Member(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            uuid = new Guid(node.Tag);

            name = node.Attributes[NAME_ATTRIBUTE];
            shortName = node.Attributes[SHORT_NAME_ATTRIBUTE];
        }
    }
}
