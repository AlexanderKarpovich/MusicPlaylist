namespace Playlist
{
    public interface IPlaylist
    {
        Task Play();
        void Pause();

        Task Prev();
        Task Next();

        Task AddSong(Song lastSong);
    }
}