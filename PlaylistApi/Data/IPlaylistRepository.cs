using Playlist;

namespace PlaylistApi.Data
{
    /// <summary>
    /// Playlist repository interface.
    /// </summary>
    public interface IPlaylistRepository
    {
        /// <summary>
        /// Tries to save repository changes.
        /// </summary>
        /// <returns><see langword="true" /> if changes saved successfully; otherwise - <see langword="false" />.</returns>
        bool SaveChanges();

        /// <summary>
        /// Gets all songs from repository.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}" /> containing all songs in playlist.</returns>
        IEnumerable<Song> GetAllSongs();

        /// <summary>
        /// Gets song by specified id (PK).
        /// </summary>
        /// <param name="id">Specified songs primary key value.</param>
        /// <returns>Specified <see cref="Song" /> exemplar if value found; otherwise - <see langword="null" />.</returns>
        Song? GetSongById(int id);

        /// <summary>
        /// Adds song to the playlist repository.
        /// </summary>
        /// <param name="song">Specified <see cref="Song" /> value.</param>
        void AddSong(Song song);

        /// <summary>
        /// Updates specified <see cref="Song" /> exemplar in repository.
        /// </summary>
        /// <param name="song">Specified <see cref="Song" /> value.</param>
        /// <returns><see langword="true" /> if value successfully updated; otherwise - <see langword="false" />.</returns>
        bool UpdateSong(Song song);

        /// <summary>
        /// Removes <see cref="Song" /> exemplar from repository by specified <paramref name="id" /> value.
        /// </summary>
        /// <param name="id">Specified primary key value.</param>
        void RemoveSong(int id);
    }
}