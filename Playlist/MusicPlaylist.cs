using Playlist.Data;

namespace Playlist
{
    /// <summary>
    /// Represents music playlist containing songs to play.
    /// </summary>
    public class MusicPlaylist : IPlaylist, IDisposable
    {
        private readonly Task playTask;
        private readonly CancellationTokenSource tokenSource;
        private readonly CancellationToken cancellationToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicPlaylist" /> class that contains elements copied from
        /// the specified <see cref="IEnumerable{Song}" /> to the <see cref="MusicPlaylist.Songs" /> property.
        /// </summary>
        /// <param name="songs">The <see cref="IEnumerable{Song}" /> whose elements are copied to the <see cref="MusicPlaylist.Songs" /> property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="songs" /> is <see langword="null" />.</exception>
        public MusicPlaylist(IEnumerable<Song> songs)
        {
            if (songs is null)
            {
                throw new ArgumentNullException("songs");
            }

            Songs = new CircularLinkedList<Song>(songs);
            CurrentSong = Songs.First();
            Playtime = 0;

            tokenSource = new();
            cancellationToken = tokenSource.Token;

            playTask = new Task(async () => await PlaySong(), cancellationToken);
        }

        /// <summary>
        /// <see cref="CircularLinkedList{T}" /> exemplar containing songs to play.
        /// </summary>
        /// <value><see cref="CircularLinkedList{T}" /></value>
        public CircularLinkedList<Song> Songs { get; }
        /// <summary>
        /// Current song in playlist.
        /// </summary>
        /// <value><see cref="Song" /></value>
        public Song CurrentSong { get; private set; }
        /// <summary>
        /// <see cref="CurrentSong" /> playtime.
        /// </summary>
        /// <value><see cref="int" /></value>
        public int Playtime { get; private set; }
        /// <summary>
        /// Indicates whether the <see cref="CurrentSong" /> playing or not.
        /// </summary>
        /// <value><see cref="bool" /></value>
        public bool IsPlaying { get; private set; }

        public async Task Play()
        {
            await Task.Run(() => playTask.Start());
        }

        public void Pause()
        {
            tokenSource.Cancel();
        }

        public async Task Prev()
        {
            await Task.Run(() =>
            {
                CurrentSong = Songs.FindNode(CurrentSong)?.Previous.Data!;
                Playtime = 0;
            });
        }

        public async Task Next()
        {
            await Task.Run(() =>
            {
                CurrentSong = Songs.FindNode(CurrentSong)?.Next.Data!;
                Playtime = 0;
            });
        }

        public async Task AddSong(Song lastSong)
        {
            await Task.Run(() =>
            {
                Songs.Add(lastSong);
            });
        }

        public void Dispose()
        {
            if (playTask.Status is not TaskStatus.RanToCompletion)
            {
                tokenSource.Cancel();
            }
            tokenSource.Dispose();
        }

        /// <summary>
        /// Plays the <see cref="MusicPlaylist.CurrentSong" /> asynchronously.
        /// </summary>
        /// <returns>The task object representing the asynchronous operation.</returns>
        private async Task PlaySong()
        {
            IsPlaying = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Playtime >= CurrentSong?.Duration)
                {
                    await Next();
                }

                await Task.Delay(1000);
                Playtime++;
            }
            IsPlaying = false;
        }
    }
}