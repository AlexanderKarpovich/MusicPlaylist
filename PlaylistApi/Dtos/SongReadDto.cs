namespace PlaylistApi.Dtos
{
    /// <summary>
    /// Represents a data transfer object for reading song data.
    /// </summary>
    public class SongReadDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public int Duration { get; set; }
    }
}