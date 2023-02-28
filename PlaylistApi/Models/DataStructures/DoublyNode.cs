namespace PlaylistApi.Models.DataStructures
{
    /// <summary>
    /// Represents a node in a <see cref="CircularLinkedList{T}" />. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Specifies the element type of the circular linked list.</typeparam>
    public sealed class DoublyNode<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoublyNode{T}" /> class, containing the specified value.
        /// </summary>
        /// <param name="data">The value to contain in the <see cref="DoublyNode{T}" />.</param>
        public DoublyNode(T data)
        {
            Data = data;
        }

        /// <summary>
        /// Specified value of type <typeparamref name="T" />.
        /// </summary>
        /// <value><typeparamref name="T" /></value>
        public T Data { get; }
        
        /// <summary>
        /// A reference to the <see cref="CircularLinkedList{T}" /> that the <see cref="DoublyNode{T}" /> belongs to, 
        /// or <see langword="null" /> if the <see cref="DoublyNode{T}" /> is not linked.
        /// </summary>
        /// <value><see cref="CircularLinkedList{T}" /></value>
        public CircularLinkedList<T> List { get; set; } = null!;

        /// <summary>
        /// A reference to the previous node in the <see cref="CircularLinkedList{T}" />, 
        /// or the last node in the <see cref="CircularLinkedList{T}" /> if the current node 
        /// is the first element <see cref="CircularLinkedList{T}.First" /> of the <see cref="CircularLinkedList{T}" />.
        /// </summary>
        /// <value><see cref="DoublyNode{T}" /></value>
        public DoublyNode<T> Previous { get; set; } = null!;

        /// <summary>
        /// A reference to the next node in the <see cref="CircularLinkedList{T}" />, 
        /// or the first node in the <see cref="CircularLinkedList{T}" /> if the current node 
        /// is the last element (<see cref="CircularLinkedList{T}.Last" />) of the <see cref="CircularLinkedList{T}" />.
        /// </summary>
        /// <value><see cref="DoublyNode{T}" /></value>
        public DoublyNode<T> Next { get; set; } = null!;
    }
}