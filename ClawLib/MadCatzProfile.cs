﻿using Claw.Blasts;
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
    public class MadCatzProfile : NodeParser
    {
        private const string VERSION_ATTRIBUTE = "version";

        internal const string CONTROLLERS_CHILD_NODE = "CONTROLLERS";
        internal const string COMMANDS_CHILD_NODE = "COMMANDS";
        internal const string BLASTS_CHILD_NODE = "BLASTS";
        
        #region Validation

        private const uint CURRENT_VERSION = 5;
        private static readonly List<uint> SUPPORTED_VERSIONS = new List<uint>(new uint [] {
            5,
        });

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
        private CommandList commands;
        private BlastList blasts;

        /// <summary>
        /// The name of the profile.
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                CheckValid(name);
                name = value;
            }
        }

        /// <summary>
        /// The blasts of this profile. Blasts are encoded icons / images.
        /// </summary>
        public BlastList Blasts
        {
            get { return blasts; }
        }

        /// <summary>
        /// The commands of this profile.
        /// </summary>
        public CommandList Commands
        {
            get { return commands; }
        }

        /// <summary>
        /// The controllers configured for this profile.
        /// </summary>
        public ControllerList Controllers
        {
            get { return controllers; }
        }

        /// <summary>
        /// Creates a new profile with the given name.
        /// </summary>
        /// <param name="profileName">The name of the profile.</param>
        public MadCatzProfile(string profileName)
            : base()
        {
            name = profileName;
            version = CURRENT_VERSION;
            controllers = new ControllerList();
            Controller[] deviceGroups = Controller.CreateAllControllers();
            foreach (Controller deviceGroup in deviceGroups)
            {
                controllers.Add(deviceGroup);
            }
            commands = new CommandList();
            blasts = new BlastList();
        }

        /// <summary>
        /// Creates a new profile from a node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The node.</param>
        internal MadCatzProfile(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            name = node.Tag;
            if (node.Attributes.ContainsKey(VERSION_ATTRIBUTE))
            {
                version = ConversionHelper.ParseHexValue(node.Attributes[VERSION_ATTRIBUTE]);
                if (!SUPPORTED_VERSIONS.Contains(version))
                {
                    validator.Report.AddError("Unsupported version: " + version);
                }
            }

            foreach (var child in node.Children)
            {
                switch (child.Name.ToUpperInvariant())
                {
                    case CONTROLLERS_CHILD_NODE:
                        controllers = new ControllerList(validator, child);
                        break;

                    case COMMANDS_CHILD_NODE:
                        commands = new CommandList(validator, child);
                        break;

                    case BLASTS_CHILD_NODE:
                        blasts = new BlastList(validator, child);
                        break;
                }
            }

            if (controllers == null)
            {
                controllers = new ControllerList();
            }
            if (commands == null)
            {
                commands = new CommandList();
            }
            if (blasts == null)
            {
                blasts = new BlastList();
            }
        }

        /// <summary>
        /// Creates the node structure for this profile.
        /// </summary>
        /// <returns>The root 'profile' node.</returns>
        internal Node CreateNodes()
        {
            var node = new Node("profile");
            node.Tag = name;
            node.Attributes.Add(VERSION_ATTRIBUTE, ConversionHelper.FormatHexUInt(version));
            if (controllers != null)
                node.Children.AddLast(controllers.CreateNodes());
            if (commands != null && commands.Count != 0)
                node.Children.AddLast(commands.CreateNodes());
            if (blasts != null && blasts.Count != 0)
                node.Children.AddLast(blasts.CreateNodes());
            return node;
        }
    }
}
