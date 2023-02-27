namespace Playlist
{
    /// <summary>
    /// Represents playlist interface.
    /// </summary>
    public interface IPlaylist
    {
        /// <summary>
        /// Starts playback of the current song.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task Play();
        /// <summary>
        /// Stops playback of the current song.
        /// </summary>
        void Pause();

        /// <summary>
        /// Switches the song to the previous one.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task Prev();

        /// <summary>
        /// Switches to the next song.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task Next();

        /// <summary>
        /// Adds the specified <see cref="Song" /> to the end of the playlist.
        /// </summary>
        /// <param name="lastSong">Specified value of the <see cref="Song" />.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task AddSong(Song lastSong);
    }
}