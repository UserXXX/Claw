using Claw.Documents;
using System;
using System.Collections.Generic;

namespace Claw.Validation
{
	/// <summary>
	/// A validator for Nodes.
	/// </summary>
	internal class NodeValidator
	{
		private ValidationReport report;
		
		/// <summary>
		/// The underlying report.
		/// </summary>
		public ValidationReport Report
		{
			get { return report; }
		}
		
		/// <summary>
		/// Creates a new NodeValidator.
		/// </summary>
		/// <param name="report">The ValidationReport to fill.</param>
		internal NodeValidator(ValidationReport report)
		{
			#if DEBUG
			
			this.report = new BranchReport(new ValidationReport[] { report, new DebugReport() });
			
			#else
			
			this.report = report;
			
			#endif
		}
		
		/// <summary>
		/// Validates a node and puts the gathered information into the report.
		/// </summary>
		/// <param name="parser">The parser to validate.</param>
		/// <param name="node">The node to validate.</param>
		internal void Validate(NodeParser parser, Node node)
		{
			// Check for unknown attributes
            foreach (var attributeName in node.Attributes.Keys)
            {
                if (!IsInStringArray(attributeName, parser.RequiredAttributes) && !IsInStringArray(attributeName, parser.OptionalAttributes))
                    report.AddWarning("Unknown attribute in \"" + node.Name + "\" node: " + attributeName + "=" + node.Attributes[attributeName]);
            }

            // Check for not appearing required attributes
            foreach (var attributeName in parser.RequiredAttributes)
            {
                if (!node.Attributes.ContainsKey(attributeName))
                    report.AddError("Missing required attribute in \"" + node.Name + "\" node: " + attributeName);
            }

            if (parser.CheckNodes)
            {
	            // Check for unknown child nodes
	            foreach (var child in node.Children)
	            {
	                string name = child.Name;
	                if (!IsInStringArray(name, parser.RequiredChildNodes) && !IsInStringArray(name, parser.OptionalChildNodes))
	                    report.AddWarning("Unknown child node in \"" + node.Name + "\" node: " + name);
	            }
	
	            // Check for not appearing required child nodes
	            foreach (string name in parser.RequiredChildNodes)
	            {
	                if (!HasChildNodeWithName(node, name))
	                    report.AddError("Missing required child node in \"" + node.Name + "\" node: " + name);
	            }
            }
            
            if (parser.TagUsageType == NodeParser.TagUsage.Required && string.IsNullOrEmpty(node.Tag))
            {
            	report.AddError("Missing required tag in \"" + node.Name + "\" node.");
            }
            else if (parser.TagUsageType == NodeParser.TagUsage.NotAllowed && !string.IsNullOrEmpty(node.Tag))
            {
            	report.AddWarning("Found tag where none is allowed in \"" + node.Name + "\" node: " + node.Tag);
            }
		}
		
		/// <summary>
        /// Checks if the given node has a child node with name <paramref name="name"/>.
        /// </summary>
        /// <param name="node">The node thats childs shall be checked.</param>
        /// <param name="name">The name to check for.</param>
        /// <returns></returns>
        private bool HasChildNodeWithName(Node node, string name)
        {
            LinkedList<Node>.Enumerator enumerator = node.Children.GetEnumerator();

            bool found = false;
            while (enumerator.MoveNext() && !found)
            {
            	found |= enumerator.Current.Name == name;
            }

            return found;
        }

        /// <summary>
        /// Checks if a given string is element of an array.
        /// </summary>
        /// <param name="text">The text to check for.</param>
        /// <param name="array">The array to search in.</param>
        /// <returns></returns>
        private bool IsInStringArray(string text, string[] array)
        {
            bool found = false;

            for (var i = 0; i < array.Length && !found; i++)
            {
            	found |= text == array[i];
            }

            return found;
        }
	}
}
