using AutoMapper;
using Playlist;

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