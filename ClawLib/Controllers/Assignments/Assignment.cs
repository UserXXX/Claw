﻿using Claw.Commands;
using Claw.Controllers.Controls;
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
        private uint identifier;

        /// <summary>
        /// Unique identifier of the control this assignment binds to.
        /// </summary>
        public uint Identifier
        {
            get { return identifier; }
            protected set { identifier = value; }
        }

        #region Validation

        internal sealed override TagUsage TagUsageType
        {
            get { return TagUsage.Required; }
        }

        #endregion

        /// <summary>
        /// Creates a new empty assignment for the given control.
        /// </summary>
        /// <param name="control">The control this assignment assigns to.</param>
        internal Assignment(Control control)
            : base()
        {
            identifier = control.Identifier;
        }

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
                identifier = ConversionHelper.ParseHexValue(node.Tag);
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
