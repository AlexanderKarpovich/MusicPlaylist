using PlaylistApi.Models;

namespace PlaylistTests
{
    public class PlaylistTests
    {
        private Playlist playlist;

        public PlaylistTests()
        {
            IEnumerable<Song> songs = new Song[]
            {
                new Song() { Name = "Gila", Author = "Beach House", Duration = 286 },
                new Song() { Name = "Call Across Rooms", Author = "Grouper", Duration = 179 },
                new Song() { Name = "The Beach", Author = "The Neighbourhood", Duration = 255 },
            };

            playlist = new Playlist(songs);
        }

        [Fact]
        public void WithNoActions_CurrentSongShouldBeGila()
        {
            Song expectedSong = playlist.Songs.First();

            Assert.Equal(expectedSong, playlist.CurrentSong);
        }

        [Fact]
        public async Task Next_ShouldBePlayingCallAcrossRoomsAsync()
        {
            Song expectedSong = playlist.Songs.FindNode(playlist.Songs.First())?.Next.Data!;

            await playlist.Next();

            Assert.Equal(playlist.CurrentSong, expectedSong);
        }

        [Fact]
        public async Task Prev_ShouldBePlayingTheBeach()
        {
            Song expectedSong = playlist.Songs.FindNode(playlist.Songs.First())?.Previous.Data!;

            await playlist.Prev();

            Assert.Equal(expectedSong, playlist.CurrentSong);
        }

        [Fact]
        public async Task AddTwoSongsInParallel_ShouldContainTwoNewSongs()
        {
            // Arrange
            Song firstSong = new Song()
            {
                Name = "Darling",
                Author = "Dreams We've Had",
                Duration = 301
            };
            Song secondSong = new Song()
            {
                Name = "Vanished",
                Author = "Crystal Castles",
                Duration = 243
            };

            // Act
            await playlist.AddSong(firstSong);
            await playlist.AddSong(secondSong);

            // Assert
            Assert.Equal(5, playlist.Songs.Count);
            Assert.True(playlist.Songs.FindNode(firstSong) is not null);
            Assert.True(playlist.Songs.FindNode(secondSong) is not null);
        }

        [Fact]
        public async Task PlayForTwoSeconds_PlaytimeShouldReturnTwo()
        {
            // Act
            await playlist.Play();

            // 100 milliseconds added to increase the playtime counter
            await Task.Delay(2100);

            playlist.Pause();

            // Assert
            Assert.Equal(2, playlist.Playtime);
        }

        [Fact]
        public async Task NextAfterPlayingForTwoSeconds_PlaytimeShouldReturnZero()
        {
            // Act
            await playlist.Play();
            await Task.Delay(2100);

            await playlist.Next();

            // Assert
            Assert.Equal(0, playlist.Playtime);
        }

        [Fact]
        public async Task PrevAfterPlayingForTwoSeconds_PlaytimeShouldReturnZero()
        {
            // Act
            await playlist.Play();
            await Task.Delay(2100);

            await playlist.Prev();

            // Assert
            Assert.Equal(0, playlist.Playtime);
        }

        [Fact]
        public void CreatingPlaylistWithSongsEqualsToNull_ShouldThrowException()
        {
            // Arrange
            Action action = () => new Playlist(null!);
            Type exceptionType = typeof(ArgumentNullException);

            // Act & Assert
            Assert.Throws(exceptionType, action);
        }
    }
}