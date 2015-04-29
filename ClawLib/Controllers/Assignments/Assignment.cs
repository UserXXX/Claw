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
    }
}
