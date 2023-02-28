using PlaylistApi.Models.DataStructures;

namespace PlaylistApi.Models
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

        /// <summary>
        /// <see cref="CircularLinkedList{T}" /> exemplar containing songs to play.
        /// </summary>
        /// <value><see cref="CircularLinkedList{T}" /></value>
        public CircularLinkedList<Song> Songs { get; }

        /// <summary>
        /// Current song in playlist.
        /// </summary>
        /// <value><see cref="Song" /></value>
        public Song CurrentSong { get; }

        /// <summary>
        /// <see cref="CurrentSong" /> playtime.
        /// </summary>
        /// <value><see cref="int" /></value>
        public int Playtime { get; }

        /// <summary>
        /// Indicates whether the <see cref="CurrentSong" /> playing or not.
        /// </summary>
        /// <value><see cref="bool" /></value>
        public bool IsPlaying { get; }
    }
}