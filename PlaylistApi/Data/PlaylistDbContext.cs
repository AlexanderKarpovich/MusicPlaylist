using Microsoft.EntityFrameworkCore;
using Playlist;

namespace PlaylistApi.Data
{
    /// <summary>
    /// Represents database context for playlist.
    /// </summary>
    public class PlaylistDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new <see cref="PlaylistDbContext" /> object with specified <see cref="DbContextOptions" />.
        /// </summary>
        /// <param name="options">Specified <see cref="DbContextOptions" /> parameter.</param>
        public PlaylistDbContext(DbContextOptions<PlaylistDbContext> options) 
            : base(options) { }
        
        /// <summary>
        /// Songs in playlist.
        /// </summary>
        /// <value><see cref="DbSet{T}" /></value>
        public DbSet<Song> Songs { get; set; } = null!;

        /// <summary>
        /// Configures the <see cref="PlaylistDbContext" /> model and related <see cref="DbSet{T}" /> properties.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for <see cref="PlaylistDbContext" />.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Song>().HasKey(s => s.Id);
            modelBuilder.Entity<Song>().Property(s => s.Name).HasMaxLength(100).IsRequired(true);
            modelBuilder.Entity<Song>().Property(s => s.Author).HasMaxLength(100).IsRequired(true);
            modelBuilder.Entity<Song>().Property(s => s.Duration).IsRequired(true);
        }
    }
}