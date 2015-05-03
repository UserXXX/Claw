using System;
using System.IO;

namespace Claw.Documents
{
    /// <summary>
    /// Factory for Mad Catz *.pr0 file documents.
    /// </summary>
    internal class DocumentFactory
    {
        private static DocumentFactory instance = new DocumentFactory();

        /// <summary>
        /// The singleton instance of the factory.
        /// </summary>
        internal static DocumentFactory Instance
        {
            get { return instance; }
        }

        private DocumentFactory() { }

        /// <summary>
        /// Loads a profile from the specified file and returns the root node.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <returns>The loaded Node.</returns>
        internal Node Load(Stream stream)
        {
            var reader = new PR0Reader(stream);
            var node = new Node(reader, false);
            reader.Close();
            return node;
        }

        /// <summary>
        /// Saves the node strcture to the given stream.
        /// </summary>
        /// <param name="node">The node to save.</param>
        /// <param name="stream">The stream to save to.</param>
        internal void Save(Node node, Stream stream)
        {
            var writer = new StreamWriter(stream);
            writer.Write(node.ToString());
            writer.Close();
        }
    }
}
