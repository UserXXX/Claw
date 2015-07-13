using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Claw.Controllers.Assignments
{
    /// <summary>
    /// List of Bands.
    /// </summary>
    public class BandList : NodeListParser<Band>
    {
        internal const string BAND_CHILD_NODE = "BAND";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = { };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = { };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
        	BAND_CHILD_NODE,
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
        /// Creates a new and empty BandList.
        /// </summary>
        internal BandList()
            : base()
        { }

        /// <summary>
        /// Creates a new BandList from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "bands" node.</param>
        internal BandList(NodeValidator validator, Node node)
            : base(validator, node)
        {
            foreach (var child in node.Children)
            {
                // Ignore the first empty tag, whatever it is for
                if (child.Name.ToUpperInvariant() == BAND_CHILD_NODE && !string.IsNullOrEmpty(child.Tag))
                {
                    Add(new Band(validator, child));
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
            var node = new Node(ButtonAssignment.BANDS_CHILD_NODE.ToLowerInvariant());
            node.Children.AddLast(new Node(BAND_CHILD_NODE.ToLowerInvariant()));
            foreach (Band band in this)
            {
                node.Children.AddLast(band.CreateNode());
            }
            return node;
        }
    }
}
