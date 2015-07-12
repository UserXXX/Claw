using Claw;
using Claw.Documents;
using Claw.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Claw
{
    /// <summary>
    /// Abstract base class for all node parsers that represent a list of nodes.
    /// </summary>
    /// <typeparam name="T">The type of child elements.</typeparam>
    public abstract class NodeListParser<T> : NodeParser, IList<T>
    {
        private LinkedList<T> elements = new LinkedList<T>();

        /// <summary>
        /// Whether this is read-only. It is not, so this will always return false.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Specifies the number of elements in this list.
        /// </summary>
        public int Count
        {
            get { return elements.Count; }
        }

        /// <summary>
        /// Gets an element by index. This is achieved by running through the list from the first element on. Be aware: This has a runtime of O(n)!
        /// </summary>
        /// <param name="index">The index to access at.</param>
        /// <returns>The element at index <paramref name="index"/>.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= elements.Count)
                    throw new ArgumentOutOfRangeException("index");

                IEnumerator<T> enumerator = elements.GetEnumerator();
                enumerator.MoveNext();
                for (var i = 0; i < index; i++)
                {
                    enumerator.MoveNext();
                }
                return enumerator.Current;
            }
            set
            {
                if (index < 0 || index >= elements.Count)
                    throw new ArgumentOutOfRangeException("index");

                LinkedListNode<T> current = elements.First;
                for (var i = 0; i < index; i++)
                {
                    current = current.Next;
                }
                current.Value = value;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal NodeListParser()
            : base()
        { }

        internal NodeListParser(NodeValidator validator, Node node)
            : base(validator, node)
        { }

        /// <summary>
        /// Gets an enumerator that runs through this list from the first to the last element.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        /// <summary>
        /// Gets an enumerator that runs through this list from the first to the last element.
        /// </summary>
        /// <returns>The enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        /// <summary>
        /// Removes <paramref name="item"/> from the list.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        public bool Remove(T item)
        {
            return elements.Remove(item);
        }

        /// <summary>
        /// Copies the contents of this list to the array.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="arrayIndex">The index to start at.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            elements.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Checks whether this list contains the given element.
        /// </summary>
        /// <param name="item">The item to look for.</param>
        /// <returns>Whether the list contains <paramref name="item"/>.</returns>
        public bool Contains(T item)
        {
            return elements.Contains(item);
        }

        /// <summary>
        /// Clears this list.
        /// </summary>
        public void Clear()
        {
            elements.Clear();
        }

        /// <summary>
        /// Adds <paramref name="item"/> to this list.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            elements.AddLast(item);
        }

        /// <summary>
        /// Removes the element at index <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= elements.Count)
                throw new ArgumentOutOfRangeException("index");

            LinkedListNode<T> current = elements.First;
            for (var i = 0; i < index; i++)
            {
                current = current.Next;
            }
            elements.Remove(current);
        }

        /// <summary>
        /// Inserts the given element at the given index. The element will have the index <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index to insert at.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, T item)
        {
            LinkedListNode<T> current = elements.First;
            for (var i = 0; i < index; i++)
            {
                current = current.Next;
            }
            elements.AddBefore(current, item);
        }

        /// <summary>
        /// Gets the index of the given element by a linear search.
        /// </summary>
        /// <param name="item">The item thats index is searched.</param>
        /// <returns>The indedx of the item or -1 if it is not contained in the list.</returns>
        public int IndexOf(T item)
        {
            int index = 0;

            LinkedListNode<T> current = elements.First;
            while (current.Next != null && !current.Value.Equals(item))
            {
                current = current.Next;
                index++;
            }

            return current.Value.Equals(item) ? index : -1;
        }
    }
}
