using PlaylistApi.Data;

namespace PlaylistApi.Models
{
    public class PlaylistFactory
    {
        public static IPlaylist CreatePlaylist(IServiceProvider provider)
        {
            var repository = provider.GetRequiredService<IPlaylistRepository>();

            return new Playlist(repository.GetAllSongs());
        }
    }
}