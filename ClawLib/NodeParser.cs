using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;

namespace Claw
{
    /// <summary>
    /// Abstract parent class for all node parsing components.
    /// </summary>
    public abstract class NodeParser
    {
    	/// <summary>
    	/// Specifies the type of usage of the tag.
    	/// </summary>
    	internal enum TagUsage
    	{
    		/// <summary>
    		/// The tag contains required data.
    		/// </summary>
    		Required,
    		
    		/// <summary>
    		/// The tag is allowed and may provide useful data.
    		/// </summary>
    		Optional,
    		
    		/// <summary>
    		/// The tag is not allowed to contain any data.
    		/// </summary>
    		NotAllowed,
    	}
    	
        /// <summary>
        /// The names of the required attributes.
        /// </summary>
        internal abstract string[] RequiredAttributes
        {
            get;
        }

        /// <summary>
        /// The names of the optional attributes.
        /// </summary>
        internal abstract string[] OptionalAttributes
        {
            get;
        }

        /// <summary>
        /// The names of the required child nodes.
        /// </summary>
        internal abstract string[] RequiredChildNodes
        {
            get;
        }

        /// <summary>
        /// The names of the optional child nodes.
        /// </summary>
        internal abstract string[] OptionalChildNodes
        {
            get;
        }
        
        /// <summary>
        /// Whether to check nodes or not, as some nodes have children specified by their attributes.
        /// </summary>
        internal virtual bool CheckNodes
        {
        	get { return true; }
        }
        
        /// <summary>
        /// Gets the type of usage of the tag.
        /// </summary>
        internal abstract TagUsage TagUsageType
        {
        	get;
        }

        /// <summary>
        /// Empty constructor for simple object creation.
        /// </summary>
        private NodeParser() { }

        /// <summary>
        /// Creates a new NodeParser. If running in DEBUG mode, the following checks are done:
        /// - unknown attributes
        /// - missing required attributes
        /// - unknown child nodes
        /// - missing required child nodes
        /// The results are printed to the Trace.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The Node that shall be parsed and checked.</param>
        internal NodeParser(NodeValidator validator, Node node)
        {
        	validator.Validate(this, node);
        }
    }
}
