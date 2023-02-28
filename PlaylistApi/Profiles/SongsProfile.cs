using AutoMapper;
using Playlist;
using PlaylistApi.Dtos;

namespace PlaylistApi.Profiles
{
    public class SongsProfile : Profile
    {
        public SongsProfile()
        {
            // Source -> Target
            CreateMap<Song, SongReadDto>(); 
            CreateMap<SongCreateDto, Song>();
        }
    }
}