using System;
using System.Collections.Generic;

namespace Claw.Documents
{
    /// <summary>
    /// Represents nodes used in the *.pr0 file format.
    /// </summary>
    internal class Node
    {
        private string name;
        private string tag;
        private Dictionary<string, string> attributes = new Dictionary<string, string>();
        private LinkedList<Node> children = new LinkedList<Node>();

        /// <summary>
        /// The nodes name.
        /// </summary>
        internal string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The tag value. This value is the entry behind the name and a '='.
        /// </summary>
        internal string Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// The attributes.
        /// </summary>
        internal Dictionary<string, string> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// The children of this node.
        /// </summary>
        internal LinkedList<Node> Children
        {
            get { return children; }
            set { children = value; }
        }

        /// <summary>
        /// Creates a new empty Node.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        internal Node(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Reads the node from the given reader.
        /// </summary>
        /// <param name="reader">The reader to read from.</param>
        /// <param name="openNodeAlreadyRead">Whether the opening of the node has already been read.</param>
        internal Node(PR0Reader reader, bool openNodeAlreadyRead)
        {
            if (!openNodeAlreadyRead)
                reader.SkipWhiteSpace(reader.ReadChar());
            // read the name of the node
            char current = reader.ReadNodeName(out name);

            if (current == PR0Constants.ASSIGN_CHARACTER) // the node has an identifying tag
                current = reader.ReadAttributeValue(out tag);

            current = reader.SkipWhiteSpace(current);

            while (current != PR0Constants.CLOSE_NODE_CHARCTER)
            {
                if (current == PR0Constants.OPEN_NODE_CHARCTER) // read a node
                {
                    children.AddLast(new Node(reader, true));
                    current = reader.ReadChar();
                }
                else // read an attribute
                {
                    string key;
                    string value;
                    current = reader.ReadAttribute(current, out key, out value);
                    attributes.Add(key, value);
                }
                current = reader.SkipWhiteSpace(current);
            }
        }
    }
}
