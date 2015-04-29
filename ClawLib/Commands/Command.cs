using Claw.Documents;
using Claw.Validation;
using System;

namespace Claw.Commands
{
    /// <summary>
    /// Represents a command, i.e. a key strike macro.
    /// </summary>
    public abstract class Command : NodeParser
    {
        protected const string NAME_ATTRIBUTE = "name";
        protected const string ICON_ATTRIBUTE = "icon";

        protected Guid identifier;
        protected string name;
        /// <summary>
        /// Uuid referring the icon, this is optional.
        /// </summary>
        protected Guid iconIdentifier;

        /// <summary>
        /// Creates a new Command from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "controller" node.</param>
        internal Command(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            identifier = new Guid(node.Tag);
            name = node.Attributes[NAME_ATTRIBUTE];

            if (node.Attributes.ContainsKey(ICON_ATTRIBUTE))
                iconIdentifier = new Guid(node.Attributes[ICON_ATTRIBUTE]);
        }
    }
}
