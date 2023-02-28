namespace PlaylistClient.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public int Duration { get; set; }
    }
}