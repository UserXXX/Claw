﻿using Claw.Documents;
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
        internal const string BLAST_CHILD_NODE = "blast";

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
        /// Creates a new BlastList from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The node.</param>
        internal BlastList(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                if (child.Name.ToLower() == BLAST_CHILD_NODE)
                {
                    Add(new Blast(validator, child));
                }
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        internal Node CreateNodes()
        {
            var node = new Node(Profile.BLASTS_CHILD_NODE);
            foreach (Blast blast in elements)
            {
                node.Children.AddLast(blast.CreateNodes());
            }
            return node;
        }
    }
}
