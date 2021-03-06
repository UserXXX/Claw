﻿using Claw.Documents;
using Claw.Validation;
using System;

namespace Claw.Commands
{
    /// <summary>
    /// An action representing a single key action.
    /// </summary>
    public class Action : NodeParser
    {
        private const string DEVICE_ATTRIBUTE = "device";
        private const string TIME_ATTRIBUTE = "time";
        private const string USAGE_ATTRIBUTE = "usage";
        private const string PAGE_ATTRIBUTE = "page";
        private const string VALUE_ATTRIBUTE = "value";

        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            DEVICE_ATTRIBUTE,
            USAGE_ATTRIBUTE,
            PAGE_ATTRIBUTE,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = {
            TIME_ATTRIBUTE,
            VALUE_ATTRIBUTE,
        };
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
			get { return TagUsage.NotAllowed; }
		}

        #endregion

        private ActionDevice device;
        /// <summary>
        /// Optional.
        /// </summary>
        private uint time = 0;
        private ActionKey usage;
        /// <summary>
        /// Always 0x00000007. Don't know why or what for this is.
        /// </summary>
        private uint page = 0x00000007;
        /// <summary>
        /// Optional.
        /// </summary>
        private ActionValue value = ActionValue.Released;

        /// <summary>
        /// Creates a new Action.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "action" node.</param>
        internal Action(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            if (node.Attributes.ContainsKey(DEVICE_ATTRIBUTE))
            {
                device = ActionDeviceHelper.TryParse(node.Attributes[DEVICE_ATTRIBUTE]);
            }
            if (node.Attributes.ContainsKey(TIME_ATTRIBUTE))
            {
                time = ConversionHelper.ParseHexValue(node.Attributes[TIME_ATTRIBUTE]);
            }
            if (node.Attributes.ContainsKey(USAGE_ATTRIBUTE))
            {
                usage = (ActionKey)ConversionHelper.ParseHexValue(node.Attributes[USAGE_ATTRIBUTE]);
            }
            if (node.Attributes.ContainsKey(PAGE_ATTRIBUTE))
            {
                page = ConversionHelper.ParseHexValue(node.Attributes[PAGE_ATTRIBUTE]);
            }
            if (node.Attributes.ContainsKey(VALUE_ATTRIBUTE))
            {
                value = (ActionValue)ConversionHelper.ParseHexValue(node.Attributes[VALUE_ATTRIBUTE]);
            }
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Profile files need lowercase strings.")]
        internal Node CreateNodes()
        {
            var node = new Node(ActionBlock.ACTION_CHILD_NODE.ToLowerInvariant());
            node.Attributes.Add(DEVICE_ATTRIBUTE, ActionDeviceHelper.ToString(device));
            if (time != 0)
            {
                node.Attributes.Add(TIME_ATTRIBUTE, ConversionHelper.FormatHexUInt(time));
            }
            node.Attributes.Add(USAGE_ATTRIBUTE, ConversionHelper.FormatHexUInt((uint)usage));
            node.Attributes.Add(PAGE_ATTRIBUTE, ConversionHelper.FormatHexUInt(page));
            if (value != 0)
            {
                node.Attributes.Add(VALUE_ATTRIBUTE, ConversionHelper.FormatHexUInt((uint)value));
            }
            return node;
        }
    }
}
