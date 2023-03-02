namespace PlaylistApiTests;

public class PlaylistApiTests
{
    private readonly Mock<IPlaylistRepository> mockRepository;
    private readonly IMapper serviceMapper;
    private readonly IPlaylist playlist;

    private readonly List<Song> songs;

    private readonly IMapper responseMapper;

    public PlaylistApiTests()
    {
        songs = new List<Song>()
        {
            new Song() { Id = 1, Name = "Gila", Author = "Beach House", Duration = 2 },
            new Song() { Id = 2, Name = "Call Across Rooms", Author = "Grouper", Duration = 5 },
            new Song() { Id = 3, Name = "The Beach", Author = "The Neighbourhood", Duration = 6 }
        };

        mockRepository = CreateRepository();
        serviceMapper = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile(new SongsProfile());
        }).CreateMapper();
        playlist = new Playlist(songs);

        responseMapper = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile(new SongsTestProfile());
        }).CreateMapper();
    }

    [Fact]
    public async Task GetAllSongs_ShouldReturnAllSongs()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        // Act
        var response = await service.GetAllSongs(new EmptyRequest(), TestServerCallContext.Create());

        // Assert
        mockRepository.Verify(r => r.GetAllSongs(), Times.Once);
        Assert.Equal(songs, responseMapper.Map<IEnumerable<Song>>(response.Songs));
    }

    [Fact]
    public async Task GetSongById_ShouldReturnSong()
    {
        // Arrange
        var index = 1;
        var expectedSong = songs.FirstOrDefault(s => s.Id == index);

        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        // Act
        var response = await service.GetSongById(new GetByIdRequest { Id = index }, TestServerCallContext.Create());

        // Assert
        mockRepository.Verify(r => r.GetSongById(It.IsAny<int>()), Times.Once);
        Assert.Equal(expectedSong, responseMapper.Map<Song>(response));
    }

    [Fact]
    public async Task GetSongByIdWithBadRequest_ShouldThrowRpcException_StatusCodeNotFound()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        var index = -1;
        var action = async () => await service.GetSongById(new GetByIdRequest { Id = index }, TestServerCallContext.Create());
        Type exceptionType = typeof(RpcException);

        // Act & Assert
        RpcException? exception = await Assert.ThrowsAsync(exceptionType, action) as RpcException;
        Assert.Equal(exception?.StatusCode, StatusCode.NotFound);
    }

    [Fact]
    public async Task CreateSong_SongsCountShouldReturnFour()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        var createRequest = new CreateSongRequest()
        {
            Name = "New song",
            Author = "Author",
            Duration = 1
        };

        // Act
        var response = await service.CreateSong(createRequest, TestServerCallContext.Create());

        // Assert
        mockRepository.Verify(r => r.AddSong(It.IsAny<Song>()), Times.Once);
        mockRepository.Verify(r => r.SaveChanges(), Times.Once);
        Assert.Equal(4, songs.ToList().Count);
    }

    [Fact]
    public async Task CreateSong_SongsShouldContainAddedSong()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        var addedSong = new Song()
        {
            Id = 4,
            Name = "New song",
            Author = "New author",
            Duration = 5
        };
        var createRequest = new CreateSongRequest()
        {
            Name = addedSong.Name,
            Author = addedSong.Author,
            Duration = addedSong.Duration
        };

        // Act
        var response = await service.CreateSong(createRequest, TestServerCallContext.Create());

        // Assert
        mockRepository.Verify(r => r.AddSong(It.IsAny<Song>()), Times.Once);
        mockRepository.Verify(r => r.SaveChanges(), Times.Once);
        Assert.Equal(addedSong, responseMapper.Map<Song>(response));
        Assert.Contains(addedSong, songs);
    }

    [Fact]
    public async Task UpdateSong_SongShouldBeUpdated()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        var expectedSong = new Song()
        {
            Id = 1,
            Name = "New name",
            Author = "New author",
            Duration = 1
        };
        var updateRequest = new UpdateSongRequest()
        {
            Id = 1,
            Name = "New name",
            Author = "New author",
            Duration = 1
        };

        // Act
        await service.UpdateSong(updateRequest, TestServerCallContext.Create());

        // Assert
        mockRepository.Verify(r => r.UpdateSong(It.IsAny<Song>()), Times.Once);
        mockRepository.Verify(r => r.SaveChanges(), Times.Once);
        Assert.Equal(expectedSong, songs.FirstOrDefault(s => s.Id == expectedSong.Id));
    }

    [Fact]
    public async Task UpdateSongWithBadRequest_ShouldThrowRpcException_StatusCodeNotFound()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        var action = async () => await service.UpdateSong(new UpdateSongRequest(), TestServerCallContext.Create());
        Type exceptionType = typeof(RpcException);

        // Act & Assert
        RpcException? exception = await Assert.ThrowsAsync(exceptionType, action) as RpcException;
        Assert.Equal(exception?.StatusCode, StatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteSong_SongShouldBeDeleted()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        var index = 1;
        var song = songs.FirstOrDefault(s => s.Id == index) ?? new Song();
        var deleteRequest = new DeleteSongRequest { Id = index };

        // Act
        var response = await service.DeleteSong(deleteRequest, TestServerCallContext.Create());

        // Assert
        mockRepository.Verify(r => r.GetSongById(It.IsAny<int>()));
        mockRepository.Verify(r => r.RemoveSong(It.IsAny<int>()));
        mockRepository.Verify(r => r.SaveChanges());
        
        Assert.Null(songs.FirstOrDefault(s => s.Id == index));
        Assert.DoesNotContain(song, songs);
    }

    [Fact]
    public void DeleteNonExistentSong_ShouldThrowRpcException_StatusCodeNotFound()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        var index = -1;
        var deleteRequest = new DeleteSongRequest { Id = index };

        var action = async () => await service.DeleteSong(deleteRequest, TestServerCallContext.Create());
        Type exceptionType = typeof(RpcException);

        // Act & Assert
        Assert.ThrowsAsync(exceptionType, action);
    }

    [Fact]
    public async Task DeleteCurrentlyPlayingSong_ShouldThrowRpcException_StatusCodeInvalidArgument()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        var index = 1;
        var deleteRequest = new DeleteSongRequest { Id = index };

        var action = async () => await service.DeleteSong(deleteRequest, TestServerCallContext.Create());
        Type exceptionType = typeof(RpcException);

        // Act
        await playlist.Play();

        // Act & Assert
        RpcException? exception = await Assert.ThrowsAsync(exceptionType, action) as RpcException;
        Assert.Equal(exception?.StatusCode, StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task PlaySong_IsPlayingShouldReturnTrue()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        // Act
        var response = await service.Play(new EmptyRequest(), TestServerCallContext.Create());

        // Wait for 100 milliseconds so IsPlaying property could change to true
        await Task.Delay(100);

        // Assert
        Assert.True(playlist.IsPlaying);
        Assert.Equal(playlist.CurrentSong, responseMapper.Map<Song>(response));

        // Release resources
        playlist.Pause();
    }

    [Fact]
    public async Task PausePlaying_IsPlayingShouldReturnFalse()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        // Act
        await playlist.Play();

        var response = await service.Pause(new EmptyRequest(), TestServerCallContext.Create());

        // Assert
        Assert.False(playlist.IsPlaying);
        Assert.Equal(playlist.CurrentSong, responseMapper.Map<Song>(response));
    }

    [Fact]
    public async Task SwitchToNextSong_CurrentSongShouldReturnNextSong()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);
        Song nextSong = playlist.Songs.FindNode(playlist.CurrentSong)?.Next.Data!;

        // Act
        var response = await service.Next(new EmptyRequest(), TestServerCallContext.Create());

        // Assert
        Assert.Equal(nextSong, playlist.CurrentSong);
        Assert.Equal(playlist.CurrentSong, responseMapper.Map<Song>(response));
    }

    [Fact]
    public async Task SwitchToPreviousSong_CurrentSongShouldReturnPreviousSong()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);
        Song prevSong = playlist.Songs.FindNode(playlist.CurrentSong)?.Previous.Data!;

        // Act
        var response = await service.Prev(new EmptyRequest(), TestServerCallContext.Create());

        // Assert
        Assert.Equal(prevSong, playlist.CurrentSong);
        Assert.Equal(playlist.CurrentSong, responseMapper.Map<Song>(response));
    }

    [Fact]
    public async Task AddSongToPlaylist_ShouldReturnListOfSongsInPlaylist()
    {
        // Arrange
        var service = new GrpcPlaylistService(mockRepository.Object, serviceMapper, playlist);

        var request = new CreateSongRequest()
        {
            Name = "New song",
            Author = "Author",
            Duration = 1
        };

        // Act
        var response = await service.AddSong(request, TestServerCallContext.Create());

        // Assert
        Assert.Equal(4, response.Songs.Count);
        Assert.Equal(songs, responseMapper.Map<IEnumerable<Song>>(response.Songs));
        foreach (Song item in responseMapper.Map<IEnumerable<Song>>(response.Songs))
        {
            Assert.Contains(item, playlist.Songs);
        }
    }

    private Mock<IPlaylistRepository> CreateRepository()
    {
        var repository = new Mock<IPlaylistRepository>();

        SetupRepository(repository);

        return repository;
    }

    private void SetupRepository(Mock<IPlaylistRepository> repository)
    {
        repository
            .Setup(r => r.GetAllSongs())
            .Returns(songs);

        repository
            .Setup(r => r.GetSongById(It.IsAny<int>()))
            .Returns<int>((id) => songs.FirstOrDefault(s => s.Id == id));

        repository
            .Setup(r => r.AddSong(It.IsAny<Song>()))
            .Callback<Song>((s) =>
            {
                if (s.Id == 0)
                {
                    s.Id = songs.Select(s => s.Id).Max() + 1;
                }

                songs.Add(s);
            });

        repository
            .Setup(r => r.RemoveSong(It.IsAny<int>()))
            .Callback<int>((id) =>
            {
                var songToRemove = songs.FirstOrDefault(s => s.Id == id) ?? new Song();
                songs.Remove(songToRemove);
            });

        repository
            .Setup(r => r.SaveChanges())
            .Returns(true);

        repository
            .Setup(r => r.SongExists(It.IsAny<int>()))
            .Returns<int>((id) => songs.FirstOrDefault(s => s.Id == id) is not null);

        repository
            .Setup(r => r.UpdateSong(It.IsAny<Song>()))
            .Returns<Song>((s) =>
            {
                var songToUpdate = songs.FirstOrDefault(song => song.Id == s.Id);

                if (songToUpdate is not null)
                {
                    var index = songs.IndexOf(songToUpdate);
                    songs[index] = s;

                    return true;
                }

                return false;
            });
    }
}