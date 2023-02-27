using Playlist.Data;

namespace Playlist
{
    public class MusicPlaylist : IPlaylist, IDisposable
    {
        private readonly Task playTask;
        private readonly CancellationTokenSource tokenSource;
        private readonly CancellationToken cancellationToken;

        public MusicPlaylist(IEnumerable<Song> songs)
        {
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

        private async Task PlaySong()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Playtime >= CurrentSong?.Duration)
                {
                    await Next();
                }

                await Task.Delay(1000);
                Playtime++;
            }
        }
    }
}