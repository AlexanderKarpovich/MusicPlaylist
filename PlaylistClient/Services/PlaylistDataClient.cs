using AutoMapper;
using Grpc.Net.Client;
using PlaylistApi;
using PlaylistClient.Models;

namespace PlaylistClient.Services
{
    public class PlaylistDataClient : IPlaylistDataClient
    {
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public PlaylistDataClient(IConfiguration configuration, IMapper mapper)
        {
            this.configuration = configuration;
            this.mapper = mapper;
        }

        public IEnumerable<Song>? GetAllSongs()
        {
            var client = GetClient();
            var request = new EmptyRequest();

            try
            {
                var response = client.GetAllSongs(request);
                var songs = mapper.Map<IEnumerable<Song>>(response.Songs);

                Console.WriteLine("Grpc connection established. All songs:");
                foreach (Song song in songs)
                {
                    Console.WriteLine($"\t{song.Id}. {song.Name} - {song.Author}. Duration: {song.Duration / 60}m {song.Duration % 60}s");
                }

                return songs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");
                return null;
            }
        }

        public Song? GetSongById(int id)
        {
            var client = GetClient();
            var request = new GetByIdRequest() { Id = id };

            try
            {
                var response = client.GetSongById(request);
                var song = mapper.Map<Song>(response);

                Console.WriteLine("Grpc connection established. Received song:");
                Console.WriteLine($"\t{song.Id}. {song.Name} - {song.Author}. Duration: {song.Duration / 60}m {song.Duration % 60}s");

                return song;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");
                return null;
            }
        }

        public Song? CreateSong(Song song)
        {
            var client = GetClient();
            var request = mapper.Map<CreateSongRequest>(song);

            try
            {
                var response = client.CreateSong(request);
                var receivedSong = mapper.Map<Song>(response);

                Console.WriteLine("Grpc connection established. Song created:");
                Console.WriteLine($"\t{receivedSong.Id}. {receivedSong.Name} - {receivedSong.Author}. "
                    + $"Duration: {receivedSong.Duration / 60}m {receivedSong.Duration % 60}s");

                return receivedSong;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");
                return null;
            }
        }

        public void UpdateSong(int id, Song song)
        {
            var client = GetClient();
            var request = mapper.Map<UpdateSongRequest>(song);
            request.Id = id;

            try
            {
                var response = client.UpdateSong(request);

                Console.WriteLine("Grpc connection established. Song updated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");
            }
        }

        public void DeleteSong(int id)
        {
            var client = GetClient();
            var request = new DeleteSongRequest() { Id = id };

            try
            {
                var response = client.DeleteSong(request);

                Console.WriteLine("Grpc connection established. Song deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");
            }
        }

        public Song? PlaySong()
        {
            var client = GetClient();
            var request = new EmptyRequest();

            try
            {
                var response = client.Play(request);
                var song = mapper.Map<Song>(response);

                Console.WriteLine($"Grpc connection established. Currently playing: {song.Name} by {song.Author}.");

                return song;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");

                return null;
            }
        }

        public Song? Pause()
        {
            var client = GetClient();
            var request = new EmptyRequest();

            try
            {
                var response = client.Pause(request);
                var song = mapper.Map<Song>(response);

                Console.WriteLine($"Grpc connection established. Currently stopped on: {song.Name} by {song.Author}.");

                return song;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");

                return null;
            }
        }
        
        public Song? Next()
        {
            var client = GetClient();
            var request = new EmptyRequest();

            try
            {
                var response = client.Next(request);
                var song = mapper.Map<Song>(response);

                Console.WriteLine($"Grpc connection established. Current song: {song.Name} by {song.Author}.");

                return song;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");

                return null;
            }
        }

        public Song? Prev()
        {
            var client = GetClient();
            var request = new EmptyRequest();

            try
            {
                var response = client.Prev(request);
                var song = mapper.Map<Song>(response);

                Console.WriteLine($"Grpc connection established. Current song: {song.Name} by {song.Author}.");

                return song;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");

                return null;
            }
        }

        public Song? AddSong(Song song)
        {
            var client = GetClient();
            var request = mapper.Map<CreateSongRequest>(song);

            try
            {
                var response = client.AddSong(request);
                var addedSong = mapper.Map<Song>(response);

                Console.WriteLine($"Grpc connection established. Song added: {addedSong.Name} by {addedSong.Author}.");

                return song;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grpc error: {ex.Message}");

                return null;
            }
        }

        private PlaylistService.PlaylistServiceClient GetClient()
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback 
                = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress(configuration["GrpcServer"] ?? "", 
                new GrpcChannelOptions { HttpHandler = httpHandler });
            return new PlaylistService.PlaylistServiceClient(channel);
        }
    }
}