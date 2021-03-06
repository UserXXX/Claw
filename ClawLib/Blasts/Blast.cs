﻿using Claw.Documents;
using Claw.Validation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Claw.Blasts
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

        #region Validation

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
        /// <summary>
        /// The UUID used to identify this blast.
        /// </summary>
        internal Guid Uuid
        {
            get { return uuid; }
        }

        private byte[] data;

        /// <summary>
        /// Creates a new Blast from the given node.
        /// </summary>
        /// <param name="validator">The validator to use for validation.</param>
        /// <param name="node">The "blast" node.</param>
        internal Blast(NodeValidator validator, Node node)
        	: base(validator, node)
        {
            if (!string.IsNullOrEmpty(node.Tag))
                uuid = new Guid(node.Tag);

            if (node.Attributes.ContainsKey(DATA_ATTRIBUTE))
            {
                data = Convert.FromBase64String(node.Attributes[DATA_ATTRIBUTE]);
            }
        }

        /// <summary>
        /// Creates a new blast and generates a new UUID for it. 
        /// </summary>
        /// <param name="pngData">The image data in png format.</param>
        public Blast(byte[] pngData)
        {
            uuid = Guid.NewGuid();
            data = pngData;
        }

        /// <summary>
        /// Gets the images data.
        /// </summary>
        /// <returns>A copy of the image data read from the profile file.</returns>
        public byte[] GetData()
        {
            return (byte[])data.Clone();
        }

        /// <summary>
        /// Sets the images data. This will be encoded to Base64 and written to the profile file.
        /// </summary>
        /// <param name="newData">The data of the image in PNG image format.</param>
        public void SetData(byte[] newData)
        {
            data = newData;
        }

        /// <summary>
        /// Creates the node structure.
        /// </summary>
        /// <returns>The node.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification="Necessary for outputting syntactically correct profile files.")]
        internal Node CreateNodes()
        {
            var node = new Node(BlastList.BLAST_CHILD_NODE.ToLowerInvariant());
            if (uuid != null)
            {
                node.Tag = uuid.ToString();
            }
            if (data != null)
            {
                string encoded = Convert.ToBase64String(data);
                node.Attributes.Add(DATA_ATTRIBUTE, encoded);
            }
            return node;
        }
    }
}
