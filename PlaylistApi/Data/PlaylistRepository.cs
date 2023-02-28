using PlaylistApi.Models;

namespace PlaylistApi.Data
{
    /// <summary>
    /// Represents repository with main CRUD operations for playlist.
    /// </summary>
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly PlaylistDbContext context;

        /// <summary>
        /// Initializes new <see cref="PlaylistRepository" /> instance with given <paramref name="context" />.
        /// </summary>
        /// <param name="context">Specified <see cref="PlaylistDbContext" /> exemplar.</param>
        public PlaylistRepository(PlaylistDbContext context)
        {
            this.context = context;
        }

        public void AddSong(Song song)
        {
            if (song is null)
            {
                throw new ArgumentNullException("song");
            }

            context.Add(song);
        }

        public IEnumerable<Song> GetAllSongs()
        {
            return context.Songs;
        }

        public Song? GetSongById(int id)
        {
            return context.Songs.Find(id);
        }

        public void RemoveSong(int id)
        {
            Song? song = context.Songs.Find(id);
            if (song is not null)
            {
                context.Songs.Remove(song);
            }
        }

        public bool UpdateSong(Song song)
        {
            if (song is null)
            {
                throw new ArgumentNullException("song");
            }

            if (!SongExists(song.Id))
            {
                return false;
            }
            
            context.Songs.Update(song);
            return true;
        }

        public bool SongExists(int id)
        {
            return context.Songs.Find(id) is not null;
        }

        public bool SaveChanges()
        {
            return (context.SaveChanges() > 0);
        }
    }
}