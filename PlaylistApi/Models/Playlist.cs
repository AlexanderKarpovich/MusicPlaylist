using PlaylistApi.Models.DataStructures;

namespace PlaylistApi.Models
{
    /// <summary>
    /// Represents music playlist containing songs to play.
    /// </summary>
    public class Playlist : IPlaylist, IDisposable
    {
        private readonly Task playTask;
        private readonly CancellationTokenSource tokenSource;
        private readonly CancellationToken cancellationToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="Playlist" /> class that contains elements copied from
        /// the specified <see cref="IEnumerable{Song}" /> to the <see cref="Playlist.Songs" /> property.
        /// </summary>
        /// <param name="songs">The <see cref="IEnumerable{Song}" /> whose elements are copied to the <see cref="Playlist.Songs" /> property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="songs" /> is <see langword="null" />.</exception>
        public Playlist(IEnumerable<Song> songs)
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

        public CircularLinkedList<Song> Songs { get; }
        
        public Song CurrentSong { get; private set; }
        
        public int Playtime { get; private set; }
        
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
        /// Plays the <see cref="Playlist.CurrentSong" /> asynchronously.
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