using AutoMapper;
using PlaylistApi.Models;

namespace PlaylistApi.Profiles
{
    public class SongsProfile : Profile
    {
        public SongsProfile()
        {
            // Source -> Target
            CreateMap<Song, SongResponse>();
            CreateMap<CreateSongRequest, Song>();
            CreateMap<UpdateSongRequest, Song>();
        }
    }
}