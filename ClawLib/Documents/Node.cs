using System;
using System.Collections.Generic;
using System.Text;

namespace Claw.Documents
{
    /// <summary>
    /// Represents nodes used in the *.pr0 file format.
    /// </summary>
    internal class Node
    {
        /// <summary>
        /// Signs per line in an "data" attribute.
        /// </summary>
        private const int DATA_SIGNS_PER_LINE = 76;

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

        public override string ToString()
        {
            string ret = "[" + name;
            if (!string.IsNullOrEmpty(tag))
            {
                ret += "=" + tag;
            }
            bool hasDataAttr = false;
            foreach (string attributeName in attributes.Keys)
            {
                if (attributeName != "data")
                {
                    string attributeValue = attributes[attributeName];
                    if (attributeValue.Contains(" "))
                    {
                        ret += " " + attributeName + "='" + attributeValue + "'";
                    }
                    else
                    {
                        ret += " " + attributeName + "=" + attributeValue;
                    }
                }
                else
                {
                    string value = attributes["data"];
                    ret += "\ndata<" + value.Length + "\n";
                    for (var i = 0; i < value.Length / DATA_SIGNS_PER_LINE; i++)
                    {
                        ret += value.Substring(i * DATA_SIGNS_PER_LINE, DATA_SIGNS_PER_LINE) + "\n";
                    }
                    ret += value.Substring((value.Length / DATA_SIGNS_PER_LINE) * DATA_SIGNS_PER_LINE) + "\n>";
                    hasDataAttr = true;
                }
            }
            if (!ret.EndsWith("\n"))
            {
                ret += "\n";
            }
            foreach (Node child in children)
            {
                string childString = child.ToString();
                string[] splitted = childString.Split(new char[] { '\n' });
                bool inDataAttribute = false;
                childString = "";
                for (var i = 0; i < splitted.Length; i++)
                {
                    if (inDataAttribute)
                    {
                        if (splitted[i].StartsWith(">"))
                        {
                            inDataAttribute = false;
                        }
                    }
                    else
                    {
                        if (splitted[i].StartsWith("data<"))
                        {
                            inDataAttribute = true;
                        }
                        else
                        {
                            splitted[i] = "  " + splitted[i];
                        }
                    }
                    childString += splitted[i] + "\n";
                }
                ret += childString;
            }
            // Trim away the last newline
            if (!hasDataAttr)
            {
                ret = ret.TrimEnd(new char[] { '\n' });
            }
            return ret + "]";
        }
    }
}
