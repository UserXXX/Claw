using Claw.Documents;
using Claw.Validation;
using System;

namespace Claw.Controllers.Controls
{
    /// <summary>
    /// Abstract parent class for all controls (buttons, sliders (buttons with multiple states), mousepointers).
    /// </summary>
    public abstract class Control : NodeParser
    {
        protected const string NameAttribute = "name";

        private uint identifier;
        private string name;
        
        #region Validation
        
        internal sealed override TagUsage TagUsageType
		{
			get { return TagUsage.Required; }
		}
        
        #endregion

        /// <summary>
        /// The controls unique identifier.
        /// </summary>
        internal uint Identifier
        {
            get { return identifier; }
        }

        /// <summary>
        /// The name of the control. This is read-only.
        /// </summary>
        public string Name
        {
            get { return name; }
        }
    
        /// <summary>
        /// Creates a new Control.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The control node.</param>
        internal Control(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            if (!string.IsNullOrEmpty(node.Tag))
            {
                identifier = ConversionHelper.ParseHexValue(node.Tag);
            }
            if (node.Attributes.ContainsKey(NameAttribute))
            {
                name = node.Attributes[NameAttribute];
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
            if (name != null)
            {
                node.Attributes.Add(NameAttribute, name);
            }
            FillNode(node);
            return node;
        }

        /// <summary>
        /// Name of the node in *pr0 files.
        /// </summary>
        protected abstract string NodeName
        {
            get;
        }

        /// <summary>
        /// Fills the node with data.
        /// </summary>
        /// <param name="node">The node to fill.</param>
        internal abstract void FillNode(Node node);

        public override bool Equals(object obj)
        {
            Control control = obj as Control;
            if (control != null)
            {
                return Equals(control);
            }

            return false;
        }

        /// <summary>
        /// Compares the given conrol to this control.
        /// </summary>
        /// <param name="control">The other control.</param>
        /// <returns>Whether the given control is equal to this control.</returns>
        public bool Equals(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            return control.identifier == identifier && control.name == name;
        }

        public override int GetHashCode()
        {
            return identifier.GetHashCode() + name.GetHashCode();
        }
    }
}
