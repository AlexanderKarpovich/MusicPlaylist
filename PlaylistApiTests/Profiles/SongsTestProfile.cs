namespace PlaylistApiTests.Profiles
{
    public class SongsTestProfile : Profile
    {
        public SongsTestProfile()
        {
            // Source -> Target
            CreateMap<SongResponse, Song>();
            CreateMap<Song, CreateSongRequest>();
            CreateMap<Song, UpdateSongRequest>();
        }
    }
}