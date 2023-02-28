using AutoMapper;
using Grpc.Core;
using Playlist;
using PlaylistApi.Data;

namespace PlaylistApi.Services
{
    public class GrpcPlaylistService : PlaylistService.PlaylistServiceBase
    {
        private readonly IPlaylistRepository repository;
        private readonly IMapper mapper;
        private readonly MusicPlaylist playlist;

        public GrpcPlaylistService(IPlaylistRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;

            playlist = new MusicPlaylist(repository.GetAllSongs());
        }

        public override Task<GetAllResponse> GetAllSongs(EmptyRequest request, ServerCallContext context)
        {
            var response = new GetAllResponse();
            var songs = repository.GetAllSongs();

            foreach (Song song in songs)
            {
                response.Songs.Add(mapper.Map<SongResponse>(song));
            }

            return Task.FromResult(response);
        }

        public override Task<SongResponse> GetSongById(GetByIdRequest request, ServerCallContext context)
        {
            var song = repository.GetSongById(request.Id);

            if (song is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Song not found"));
            }

            return Task.FromResult(mapper.Map<SongResponse>(song));
        }

        public override Task<SongResponse> CreateSong(CreateSongRequest request, ServerCallContext context)
        {
            var song = mapper.Map<Song>(request);

            repository.AddSong(song);
            repository.SaveChanges();

            var response = mapper.Map<SongResponse>(song);

            return Task.FromResult(response);
        }

        public override Task<EmptyResponse> UpdateSong(UpdateSongRequest request, ServerCallContext context)
        {
            var song = mapper.Map<Song>(request);

            if (!repository.UpdateSong(song))
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Song not found"));
            }

            repository.SaveChanges();
            return Task.FromResult(new EmptyResponse());
        }

        public override Task<EmptyResponse> DeleteSong(DeleteSongRequest request, ServerCallContext context)
        {
            var song = repository.GetSongById(request.Id);

            if (song is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Song not found"));
            }

            if (playlist.CurrentSong.Equals(song) && playlist.IsPlaying)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Cannot delete currently playing song"));
            }

            repository.RemoveSong(song.Id);
            repository.SaveChanges();
            return Task.FromResult(new EmptyResponse());
        }

        public override async Task<SongResponse> Play(EmptyRequest request, ServerCallContext context)
        {
            await playlist.Play();
            return mapper.Map<SongResponse>(playlist.CurrentSong);
        }

        public override Task<SongResponse> Pause(EmptyRequest request, ServerCallContext context)
        {
            playlist.Pause();
            return Task.FromResult(mapper.Map<SongResponse>(playlist.CurrentSong));
        }

        public override async Task<SongResponse> Next(EmptyRequest request, ServerCallContext context)
        {
            await playlist.Next();
            return mapper.Map<SongResponse>(playlist.CurrentSong);
        }

        public override async Task<SongResponse> Prev(EmptyRequest request, ServerCallContext context)
        {
            await playlist.Prev();
            return mapper.Map<SongResponse>(playlist.CurrentSong);
        }

        public override async Task<GetAllResponse> AddSong(CreateSongRequest request, ServerCallContext context)
        {
            var song = mapper.Map<Song>(request);

            await playlist.AddSong(song);

            repository.AddSong(song);
            repository.SaveChanges();

            var response = new GetAllResponse();
            var songs = repository.GetAllSongs();

            foreach (Song s in songs)
            {
                response.Songs.Add(mapper.Map<SongResponse>(s));
            }

            return response;
        }
    }
}