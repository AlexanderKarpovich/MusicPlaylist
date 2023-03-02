using PlaylistApi.Models;

namespace PlaylistTests
{
    public class PlaylistTests
    {
        private readonly Song[] songs;
        private readonly Playlist playlist;

        public PlaylistTests()
        {
            songs = new Song[]
            {
                new Song() { Name = "Gila", Author = "Beach House", Duration = 2 },
                new Song() { Name = "Call Across Rooms", Author = "Grouper", Duration = 5 },
                new Song() { Name = "The Beach", Author = "The Neighbourhood", Duration = 6 },
            };

            playlist = new Playlist(songs);
        }

        [Fact]
        public void NoAction_CurrentSongShouldReturnFirstSong()
        {
            // Arrange
            Song expectedSong = songs[0];

            // Assert
            Assert.Equal(expectedSong, playlist.CurrentSong);
        }

        [Fact]
        public async Task CallNextOneTime_CurrentSongShouldReturnSecondSong()
        {
            // Arrange
            Song expectedSong = songs[1];

            // Act
            await playlist.Next();

            // Assert
            Assert.Equal(playlist.CurrentSong, expectedSong);
        }

        [Fact]
        public async Task TwoNextCalls_CurrentSongShouldReturnThirdSong()
        {
            // Arrange
            Song expectedSong = songs[2];

            // Act
            await playlist.Next();
            await playlist.Next();

            // Assert
            Assert.Equal(expectedSong, playlist.CurrentSong);
        }

        [Fact]
        public async Task CallPrevOneTime_CurrentSongShoudlReturnThirdSong()
        {
            // Arrange
            Song expectedSong = songs[2];

            // Act
            await playlist.Prev();

            // Assert
            Assert.Equal(expectedSong, playlist.CurrentSong);
        }

        [Fact]
        public async Task TwoPrevCalls_CurrentSongShouldReturnSecondSong()
        {
            // Arrange
            Song expectedSong = songs[1];

            // Act
            await playlist.Prev();
            await playlist.Prev();

            // Assert
            Assert.Equal(expectedSong, playlist.CurrentSong);
        }

        [Fact]
        public async Task AddSong_SongsShoudlContainAddedSong()
        {
            // Arrange
            Song firstSong = new Song()
            {
                Name = "Darling",
                Author = "Dreams We've Had",
                Duration = 301
            };

            // Act
            await playlist.AddSong(firstSong);

            // Assert
            Assert.True(playlist.Songs.FindNode(firstSong) is not null);
            Assert.Equal(4, playlist.Songs.Count);
        }

        [Fact]
        public async Task AddTwoSongs_SongsCountShouldReturnFive()
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

        public async Task PlaySong_IsPlayingShouldReturnTrue()
        {
            // Act
            await playlist.Play();

            // Assert
            Assert.True(playlist.IsPlaying);

            // Release resources
            playlist.Pause();
        }

        public async Task PlaySongThenStop_IsPlayingShouldReturnFalse()
        {
            // Act
            await playlist.Play();
            playlist.Pause();

            // Assert
            Assert.False(playlist.IsPlaying);
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
        public async Task PlayCurrentSongToEnd_CurrentSongShouldReturnNextSong()
        {
            // Arrange
            Song expectedSong = playlist.Songs.FindNode(playlist.Songs.First())?.Next.Data!;
            // Waiting for ((duration * 1000) + 100) milliseconds. 
            // 100 milliseconds added to increase the playtime counter
            int waitTime = (playlist.CurrentSong.Duration * 1000) + 100; 

            // Act
            await playlist.Play();
            // Wait for the end of current song
            await Task.Delay(waitTime);
            playlist.Pause();

            // Assert
            Assert.Equal(expectedSong, playlist.CurrentSong);
            Assert.Equal(0, playlist.Playtime);
        }

        [Fact]
        public void CreatingPlaylistWithSongsEqualsToNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            Action action = () => new Playlist(null!);
            Type exceptionType = typeof(ArgumentNullException);

            // Act & Assert
            Assert.Throws(exceptionType, action);
        }
    }
}