namespace Playlist
{
    public class Song : IEquatable<Song>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
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