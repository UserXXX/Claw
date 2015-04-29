using System;
using System.IO;
using System.Text;

namespace Claw.Documents
{
    /// <summary>
    /// Reader for *.pr0 files.
    /// </summary>
    internal class PR0Reader : StreamReader
    {
        /// <summary>
        /// Initializes a new PR0Reader.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        internal PR0Reader(Stream stream)
            : base(stream)
        { }

        /// <summary>
        /// Reads a single char from the underlying stream.
        /// </summary>
        /// <returns>The read char.</returns>
        internal char ReadChar()
        {
            if (EndOfStream)
                throw new InvalidOperationException();

            var data = new char[1];
            ReadBlock(data, 0, 1);
            return data[0];
        }

        /// <summary>
        /// Reads the name of a node
        /// </summary>
        /// <param name="name">The read name.</param>
        /// <returns>The last char read.</returns>
        internal char ReadNodeName(out string name)
        {
            var builder = new StringBuilder();
            char current = ReadChar();
            while (current != PR0Constants.ASSIGN_CHARACTER && !char.IsWhiteSpace(current) && current != PR0Constants.CLOSE_NODE_CHARCTER)
            {
                builder.Append(current);
                current = ReadChar();
            }
            name = builder.ToString();
            return current;
        }

        /// <summary>
        /// Reads the value of an attribute.
        /// </summary>
        /// <param name="value">The read value.</param>
        /// <returns>The last char read.</returns>
        internal char ReadAttributeValue(out string value)
        {
            char current = ReadChar();
            string read = null;
            if (current == PR0Constants.ENCLOSING_OPEN_CHARACTER)
                current = ReadEnclosedAttributeValue(out read);
            else
                current = ReadPlainAttributeValue(current, out read);

            value = read;
            return current;
        }

        /// <summary>
        /// Reads a plain attribute value.
        /// </summary>
        /// <param name="current">The first char read.</param>
        /// <param name="read">The read value.</param>
        /// <returns>The last char read.</returns>
        private char ReadPlainAttributeValue(char current, out string read)
        {
            var builder = new StringBuilder();

            while (!char.IsWhiteSpace(current) && current != PR0Constants.CLOSE_NODE_CHARCTER)
            {
                builder.Append(current);
                current = ReadChar();
            }

            read = builder.ToString();
            return current;
        }

        /// <summary>
        /// Reads an enclosed attribute value.
        /// </summary>
        /// <param name="read">The read value.</param>
        /// <returns>The last char read.</returns>
        private char ReadEnclosedAttributeValue(out string read)
        {
            var builder = new StringBuilder();

            char current = ReadChar();
            while (current != PR0Constants.ENCLOSING_CLOSE_CHARACTER)
            {
                builder.Append(current);
                current = ReadChar();
            }
            current = ReadChar();

            read = builder.ToString();
            return current;
        }

        /// <summary>
        /// Skips all white space characters till the first non white space character.
        /// </summary>
        /// <param name="current">First char of the white spcaed block read.</param>
        /// <returns>The first read non white space character or ' ' if the end of the stream was reached.</returns>
        internal char SkipWhiteSpace(char current)
        {
            while (char.IsWhiteSpace(current) && !EndOfStream)
                current = ReadChar();
            return current;
        }

        /// <summary>
        /// Reads an attribute (name and value).
        /// </summary>
        /// <param name="first">First char of the attributes name.</param>
        /// <param name="name">Name of the attribute.</param>
        /// <param name="value">Value of the attribute.</param>
        /// <returns>The last char read.</returns>
        internal char ReadAttribute(char first, out string name, out string value)
        {
            string pName;
            char current = ReadNodeName(out pName);
            name = first + pName;

            // Some compiled data code, handled differently.
            if (name.StartsWith("data<", StringComparison.CurrentCulture))
            {
                name = "data";
                current = SkipWhiteSpace(current);
                var builder = new StringBuilder();
                while (current != PR0Constants.TERMINATE_DATA_SECTION_CHARACTER)
                {
                    if (!char.IsWhiteSpace(current))
                        builder.Append(current);

                    current = ReadChar();
                }
                value = builder.ToString();
                // Skip the current close character.
                return ReadChar();
            }
            else
            {
                if (current != PR0Constants.ASSIGN_CHARACTER)
                    throw new InvalidOperationException("Could not read an attribute from the current position of the stream.");

                string pValue;
                current = ReadAttributeValue(out pValue);
                value = pValue;

                return current;
            }
        }
    }
}
