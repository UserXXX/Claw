using Claw.Commands;
using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Claw.Blasts
{
    /// <summary>
    /// A list of blasts (=images).
    /// </summary>
    public class BlastList : NodeListParser<Blast>
    {
        internal const string BLAST_CHILD_NODE = "BLAST";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = { };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
        	BLAST_CHILD_NODE,
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
        
        /// <summary>
        /// Default constructor. Creates an empty list.
        /// </summary>
        public BlastList()
            : base()
        { }

        /// <summary>
        /// Creates a new BlastList from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The node.</param>
        internal BlastList(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                if (child.Name.ToUpperInvariant() == BLAST_CHILD_NODE)
                {
                    Add(new Blast(validator, child));
                }
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Necessary for outputting syntactically correct profile files.")]
        internal Node CreateNodes()
        {
            var node = new Node(MadCatzProfile.BLASTS_CHILD_NODE.ToLowerInvariant());
            foreach (Blast blast in this)
            {
                node.Children.AddLast(blast.CreateNodes());
            }
            return node;
        }

        /// <summary>
        /// Gets the blast for the given identifier.
        /// </summary>
        /// <param name="identifier">The identifier to search for.</param>
        /// <returns>The blast or null if none exists.</returns>
        public Blast GetBlastForCommand(Command command)
        {
            Guid blastUuid = command.IconUuid;
            foreach (Blast blast in this)
            {
                if (blast.Uuid.Equals(blastUuid))
                {
                    return blast;
                }
            }
            return null;
        }
    }
}
