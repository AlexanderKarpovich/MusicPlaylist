using System.ComponentModel.DataAnnotations;

namespace PlaylistApi.Dtos
{
    /// <summary>
    /// Represents a data transfer object for creating a song. 
    /// </summary>
    public class SongCreateDto
    {
        [Required]
        [MaxLength(100)]
        [MinLength(10)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(10)]
        public string? Author { get; set; }

        [Required]
        [Range(30, 7200)]
        public int Duration { get; set; }
    }
}