namespace PlaylistApi.Models
{
    /// <summary>
    /// Represents a song to play in a playlist.
    /// </summary>
    public class Song : IEquatable<Song>
    {
        /// <summary>
        /// Key value.
        /// </summary>
        /// <value><see cref="int" /></value>
        public int Id { get; set; }

        /// <summary>
        /// The name of the specific song.
        /// </summary>
        /// <value><see cref="string?" /></value>
        public string? Name { get; set; }

        /// <summary>
        /// The name or pseudonym of the songwriter.
        /// </summary>
        /// <value><see cref="string?" /></value>
        public string? Author { get; set; }

        /// <summary>
        /// The duration of the song.
        /// </summary>
        /// <value><see cref="int" /></value>
        public int Duration { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Song);
        }

        public bool Equals(Song? other)
        {
            bool areEqual = other is not null && this.Id == other.Id && 
                this.Name == other.Name && this.Author == other.Author && 
                this.Duration == other.Duration;

            return areEqual;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Author, Duration);
        }
    }
}