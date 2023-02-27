using System.Collections;

namespace Playlist.Data
{
    /// <summary>
    /// Represents a circular doubly linked list.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the linked list.</typeparam>
    public class CircularLinkedList<T> : IEnumerable<T>
    {
        private DoublyNode<T> head = null!;
        private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularLinkedList{T}" /> class that is empty.
        /// </summary>
        public CircularLinkedList()
        {
            count = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularLinkedList{T}" /> class that contains elements copied from
        /// the specified <see cref="IEnumerable" /> and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="data">The <see cref="IEnumerable" /> whose elements are copied to the new <see cref="CircularLinkedList{T}" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
        public CircularLinkedList(IEnumerable<T> data) : base()
        {
            if (data is null)
            {
                throw new ArgumentNullException("Data is null.");
            }

            foreach (T item in data)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Gets the number of nodes actually contained in the <see cref="CircularLinkedList{T}" />.
        /// </summary>
        public int Count => count;
        /// <summary>
        /// Indicates whether the <see cref="CircularLinkedList{T}" /> contains at least one node.
        /// </summary>
        public bool IsEmpty => count == 0;
        /// <summary>
        /// Gets the first node of the <see cref="CircularLinkedList{T}" />.
        /// </summary>
        public DoublyNode<T> First => head;
        /// <summary>
        /// Gets the last node of the <see cref="CircularLinkedList{T}" />.
        /// </summary>
        public DoublyNode<T> Last => head.Previous;

        /// <summary>
        /// Adds a new node containing the specified value at the end of the <see cref="CircularLinkedList{T}" />.
        /// </summary>
        /// <param name="item">Specified value of type <typeparamref name="T" />.</param>
        public void Add(T item)
        {
            DoublyNode<T> node = new(item);

            if (head is null)
            {
                head = node;
                head.Next = node;
                head.Previous = node;

                head.List = this;
            }
            else
            {
                node.Previous = head.Previous;
                node.Next = head;
                node.List = this;

                head.Previous.Next = node;
                head.Previous = node; 
            }

            count++;
        }

        /// <summary>
        /// Removes the first occurrence of the specified value from <see cref="CircularLinkedList{T}" />.
        /// </summary>
        /// <param name="item">Specified value of type <typeparamref name="T" />.</param>
        /// <returns><see langword="true" /> if the element containing <see langword="value" /> is successfully removed; 
        /// otherwise, <see langword="false" />. This method also returns <see langword="false" /> if <see langword="value" /> 
        /// was not found in the original <see cref="CircularLinkedList{T}" />.</returns>
        public bool Remove(T item)
        {
            if (IsEmpty) return false;

            DoublyNode<T> itemToRemove = FindNode(item)!;

            if (itemToRemove is not null)
            {
                if (count == 1)
                {
                    head = null!;
                }
                else
                {
                    if (itemToRemove.Equals(head))
                    {
                        head = head.Next;
                    }
                    itemToRemove.Previous.Next = itemToRemove.Next;
                    itemToRemove.Next.Previous = itemToRemove.Previous;
                }
                count--;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Finds node in the <see cref="CircularLinkedList{T}" /> by the specified value.
        /// </summary>
        /// <param name="item">Specified value of type <typeparamref name="T" />.</param>
        /// <returns><see cref="DoublyNode{T}" /> element if the <see langword="value" /> is found; otherwise, <see langword="null" />.</returns>
        public DoublyNode<T>? FindNode(T item)
        {
            DoublyNode<T> current = head;

            do
            {
                if (current.Data?.Equals(item) ?? false)
                {
                    return current;
                }
                current = current.Next;
            }
            while (current != head);

            return null;
        }

        /// <summary>
        /// Removes all nodes from the <see cref="CircularLinkedList{T}" />.
        /// </summary>
        public void Clear()
        {
            head = null!;
            count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            DoublyNode<T> current = head;

            do
            {
                if (current is not null)
                {
                    yield return current.Data;
                    current = current.Next;
                }
            }
            while (current != head);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}