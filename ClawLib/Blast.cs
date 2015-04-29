using Claw.Documents;
using Claw.Validation;
using System;
using System.Drawing;
using System.IO;

namespace Claw
{
    /// <summary>
    /// Represents an icon. The icon data encoding is as folows:
    /// [blast=uuid
    /// data<number of encoded bytes
    /// Base64 encoded PNG data, 76 signs per line, one line break
    /// </summary>
    public class Blast : NodeParser
    {
        private const string DATA_ATTRIBUTE = "data";

        #region Check Stuff

        private static readonly string[] REQUIRED_ATTRIBUTES = {
            DATA_ATTRIBUTE,
        };
        private static readonly string[] OPTIONAL_ATTRIBUTES = { };
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
			get { return TagUsage.Required; }
		}

        #endregion
        
        private Guid uuid;
        private Image image;

        /// <summary>
        /// Creates a new Blast from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "blast" node.</param>
        internal Blast(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            uuid = new Guid(node.Tag);

            byte[] data = Convert.FromBase64String(node.Attributes[DATA_ATTRIBUTE]);
            using (var stream = new MemoryStream(data))
            {
            	image = new Bitmap(stream);
            }
        }
    }
}
