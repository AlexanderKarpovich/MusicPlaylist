using AutoMapper;
using PlaylistApi;
using PlaylistClient.Models;

namespace PlaylistClient.Profiles
{
    public class SongsProfile : Profile
    {
        public SongsProfile()
        {
            // Source -> Target
            CreateMap<SongResponse, Song>();
            CreateMap<Song, CreateSongRequest>();
            CreateMap<Song, UpdateSongRequest>();
        }
    }
}