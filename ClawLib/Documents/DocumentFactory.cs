using System;
using System.IO;

namespace Claw.Documents
{
    /// <summary>
    /// Container for factory methods for Mad Catz *.pr0 file documents.
    /// </summary>
    internal static class DocumentFactory
    {
        /// <summary>
        /// Loads a profile from the specified file and returns the root node.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <returns>The loaded Node.</returns>
        internal static Node Load(Stream stream)
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
        internal static void Save(Node node, Stream stream)
        {
            var writer = new StreamWriter(stream);
            writer.Write(node.ToString().Replace("\n", Environment.NewLine));
            writer.Close();
        }
    }
}
