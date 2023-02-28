using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Playlist;
using PlaylistApi.Data;
using PlaylistApi.Dtos;

namespace PlaylistApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly IPlaylistRepository repository;
        private readonly IMapper mapper;
        private readonly MusicPlaylist playlist;

        public SongsController(IPlaylistRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;

            playlist = new MusicPlaylist(repository.GetAllSongs());
        }

        [HttpGet]
        public ActionResult<IEnumerable<Song>> GetAllSongs()
        {
            var songs = repository.GetAllSongs();

            return Ok(mapper.Map<IEnumerable<SongReadDto>>(songs));
        }

        [HttpGet("{id}")]
        public ActionResult<SongReadDto> GetSongById(int id)
        {
            var song = repository.GetSongById(id);

            if (song is null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<SongReadDto>(song));
        }

        [HttpPost]
        public ActionResult<SongReadDto> CreateSong(SongCreateDto songCreateDto)
        {
            var song = mapper.Map<Song>(songCreateDto);

            repository.AddSong(song);
            repository.SaveChanges();

            var songReadDto = mapper.Map<SongReadDto>(song);

            return CreatedAtAction(nameof(GetSongById), new { Id = songReadDto.Id }, songReadDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSong(int id, SongCreateDto songCreateDto)
        {
            var song = mapper.Map<Song>(songCreateDto);
            
            song.Id = id;

            if (!repository.UpdateSong(song))
            {
                return NotFound();
            }
            
            repository.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSong(int id)
        {
            var song = repository.GetSongById(id);

            if (song is null)
            {
                return NotFound();
            }

            repository.RemoveSong(id);
            repository.SaveChanges();
            return Ok();
        }
    }
}