using Claw.Documents;
using Claw.Validation;
using System;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// Class representing an keystrike/macro assignment to a previously declared control.
    /// </summary>
    public abstract class Assignment : NodeParser
    {
        protected uint identifier;

        #region Validation

        internal sealed override TagUsage TagUsageType
        {
            get { return TagUsage.Required; }
        }

        #endregion

        /// <summary>
        /// Creates a new Assignment.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The node to parse from.</param>
        internal Assignment(NodeValidator validator, Node node)
            : base(validator, node)
        {
            if (!string.IsNullOrEmpty(node.Tag))
            {
                identifier = ConversionHelper.ParseHexUint(node.Tag);
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        internal Node CreateNodes()
        {
            var node = new Node(NodeName);
            node.Tag = ConversionHelper.FormatHexUInt(identifier);
            FillNode(node);
            return node;
        }

        /// <summary>
        /// The node name used in *.pr0 files.
        /// </summary>
        protected abstract string NodeName
        {
            get;
        }

        /// <summary>
        /// Fills th given node with data.
        /// </summary>
        /// <param name="node">The node to fill.</param>
        internal abstract void FillNode(Node node);
    }
}
