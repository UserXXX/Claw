﻿using Claw.Blasts;
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
        /// <summary>
        /// The unique identifier for this command.
        /// </summary>
        internal Guid Uuid
        {
            get { return uuid; }
        }

        private string name;
        /// <summary>
        /// The commands name.
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
        /// Uuid referring the icon, this is optional.
        /// </summary>
        private Guid iconUuid;
        /// <summary>
        /// The UUID of the icon associated with this command.
        /// </summary>
        internal Guid IconUuid
        {
            get { return iconUuid; }
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        protected Command(string commandName)
            : base()
        {
            name = commandName;
            uuid = Guid.NewGuid();
        }

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

        /// <summary>
        /// Sets the icon associated with this command.
        /// </summary>
        /// <param name="blast">The blast containing the icon data or null to specify that no icon is associated.</param>
        public void SetIcon(Blast blast)
        {
            if (blast == null)
            {
                iconUuid = Guid.Empty;
            }
            else
            {
                iconUuid = blast.Uuid;
            }
        }

        public override bool Equals(object obj)
        {
            Command command = obj as Command;

            if (command != null)
            {
                return Equals(command);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the given command is equal to this command.
        /// </summary>
        /// <param name="command">The command to compare to.</param>
        /// <returns>Whether the given command and this command are equal.</returns>
        public bool Equals(Command command)
        {
            if (command == null)
            {
                return false;
            }

            // The UUID is unique, it is enough to test for it.
            return command.uuid.Equals(uuid);
        }

        public override int GetHashCode()
        {
            return uuid.GetHashCode();
        }
    }
}
