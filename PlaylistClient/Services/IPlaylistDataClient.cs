using PlaylistClient.Models;

namespace PlaylistClient.Services
{
    public interface IPlaylistDataClient
    {
        IEnumerable<Song>? GetAllSongs();
        Song? GetSongById(int id);
        Song? CreateSong(Song song);
        void UpdateSong(int id, Song song);
        void DeleteSong(int id);

        Song? PlaySong();
        Song? Pause();
        Song? Next();
        Song? Prev();
        Song? AddSong(Song song);
    }
}