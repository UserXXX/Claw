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
        protected const string NameAttribute = "name";
        protected const string IconAttribute = "icon";

        #region Validation

        internal sealed override TagUsage TagUsageType
        {
            get { return TagUsage.Required; }
        }

        #endregion

        private Guid uuid;
        private string name;
        /// <summary>
        /// Uuid referring the icon, this is optional.
        /// </summary>
        private Guid iconUuid;

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
            if (node.Attributes.ContainsKey(NameAttribute))
            {
                name = node.Attributes[NameAttribute];
            }
            if (node.Attributes.ContainsKey(IconAttribute))
            {
                iconUuid = new Guid(node.Attributes[IconAttribute]);
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        internal Node CreateNodes()
        {
            var node = new Node(NodeName);
            if (uuid != null && uuid != Guid.Empty)
            {
                node.Tag = uuid.ToString();
            }
            if (name != null)
            {
                node.Attributes.Add(NameAttribute, name);
            }
            if (iconUuid != null && iconUuid != Guid.Empty)
            {
                node.Attributes.Add(IconAttribute, iconUuid.ToString());
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
