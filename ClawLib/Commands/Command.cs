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

        protected Guid uuid;
        protected string name;
        /// <summary>
        /// Uuid referring the icon, this is optional.
        /// </summary>
        protected Guid iconUuid;

        /// <summary>
        /// Creates a new Command from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "controller" node.</param>
        internal Command(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            if (!string.IsNullOrEmpty(node.Tag))
            {
                uuid = new Guid(node.Tag);
            }
            if (node.Attributes.ContainsKey(NAME_ATTRIBUTE))
            {
                name = node.Attributes[NAME_ATTRIBUTE];
            }
            if (node.Attributes.ContainsKey(ICON_ATTRIBUTE))
            {
                iconUuid = new Guid(node.Attributes[ICON_ATTRIBUTE]);
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        internal Node CreateNodes()
        {
            var node = new Node(NodeName);
            if (uuid != null)
            {
                node.Tag = uuid.ToString();
            }
            if (name != null)
            {
                node.Attributes.Add(NAME_ATTRIBUTE, name);
            }
            if (iconUuid != null)
            {
                node.Attributes.Add(ICON_ATTRIBUTE, iconUuid.ToString());
            }
            FillNode(node);
            return node;
        }

        /// <summary>
        /// The name of the node in *.pr0 files.
        /// </summary>
        protected abstract string NodeName
        {
            get;
        }

        /// <summary>
        /// Fills the node with data.
        /// </summary>
        /// <param name="node">Node to fill.</param>
        internal abstract void FillNode(Node node);
    }
}
