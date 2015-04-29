using System;

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
        /// <param name="filename">Name of the file to load from.</param>
        /// <returns>The loaded Node.</returns>
        internal Node Load(string filename)
        {
            var reader = new PR0Reader(filename);
            var node = new Node(reader, false);
            reader.Close();
            return node;
        }

        internal void Save(Node node, string filename)
        {
        	throw new NotImplementedException();
        }
    }
}
