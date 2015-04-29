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
        protected const string NAME_ATTRIBUTE = "name";

        protected uint identifier;
        protected string name;
        
        #region Validation
        
        internal sealed override TagUsage TagUsageType
		{
			get { return TagUsage.Required; }
		}
        
        #endregion
    
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
                identifier = ConversionHelper.ParseHexUint(node.Tag);
            }
            if (node.Attributes.ContainsKey(NAME_ATTRIBUTE))
            {
                name = node.Attributes[NAME_ATTRIBUTE];
            }
        }
    }
}
