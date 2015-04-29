using Claw.Commands;
using Claw.Controllers;
using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;

namespace Claw
{
    /// <summary>
    /// Represents a profile, the top level node.
    /// </summary>
    public class Profile : NodeParser
    {
        private const string VERSION_ATTRIBUTE = "version";

        private const string CONTROLLERS_CHILD_NODE = "controllers";
        private const string COMMANDS_CHILD_NODE = "commands";
        private const string BLASTS_CHILD_NODE = "blasts";
        
        #region Validation

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            VERSION_ATTRIBUTE,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
        private static readonly string[] REQUIRED_CHILD_NODES = {
        	CONTROLLERS_CHILD_NODE,
        };
        private static readonly string[] OPTIONAL_CHILD_NODES = {
        	COMMANDS_CHILD_NODE,
        	BLASTS_CHILD_NODE,
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
			get { return TagUsage.Required; }
		}

        #endregion
        
        private string name;
        private uint version;

        private ControllerList controllers;
        private LinkedList<Command> commands = new LinkedList<Command>();
        private LinkedList<Blast> blasts = new LinkedList<Blast>();

        /// <summary>
        /// Creates a new profile from a node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The node.</param>
        internal Profile(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            name = node.Tag;
            version = ConversionHelper.ParseHexUint(node.Attributes[VERSION_ATTRIBUTE]);

            foreach (var child in node.Children)
            {
                switch (child.Name.ToLower())
                {
                    case CONTROLLERS_CHILD_NODE:
                        controllers = new ControllerList(validator, child);
                        break;

                    case COMMANDS_CHILD_NODE:
                        LoadCommands(validator, child);
                        break;

                    case BLASTS_CHILD_NODE:
                        LoadBlasts(validator, child);
                        break;
                }
            }
        }

        /// <summary>
        /// Loads the blasts from a node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The node.</param>
        private void LoadBlasts(NodeValidator validator, Node node)
        {
            foreach (var child in node.Children)
                blasts.AddLast(new Blast(validator, child));
        }

        /// <summary>
        /// Loads the commands from a node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "commands" node.</param>
        private void LoadCommands(NodeValidator validator, Node node)
        {
            foreach (var child in node.Children)
            {
                commands.AddLast(new ActionCommand(validator, child));
            }
        }
    }
}
