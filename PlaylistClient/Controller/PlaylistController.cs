using Microsoft.AspNetCore.Mvc;
using PlaylistClient.Models;
using PlaylistClient.Services;

namespace PlaylistClient.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistDataClient dataClient;

        public PlaylistController(IPlaylistDataClient dataClient)
        {
            this.dataClient = dataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Song>> GetAllSongs()
        {
            return Ok(dataClient.GetAllSongs());
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Song>> GetSongById(int id)
        {
            return Ok(dataClient.GetSongById(1));
        }

        [HttpPost]
        public ActionResult<Song> CreateSong(Song song)
        {
            dataClient.CreateSong(song);
            return CreatedAtAction(nameof(GetSongById), new { Id = song.Id }, song);    
        }

        [HttpPut]
        public ActionResult UpdateSong(int id, Song song)
        {
            dataClient.UpdateSong(id, song);
            return Ok();
        }

        [HttpDelete]
        public ActionResult DeleteSong(int id)
        {
            dataClient.DeleteSong(id);
            return Ok();
        }

        [HttpGet("play")]
        public ActionResult<Song> Play()
        {
            return Ok(dataClient.PlaySong());
        }

        [HttpGet("pause")]
        public ActionResult<Song> Pause()
        {
            return Ok(dataClient.Pause());
        }

        [HttpGet("next")]
        public ActionResult<Song> Next()
        {
            return Ok(dataClient.Next());
        }

        [HttpGet("prev")]
        public ActionResult<Song> Prev()
        {
            return Ok(dataClient.Prev());
        }

        [HttpPost("add")]
        public ActionResult<Song> AddSong(Song song)
        {
            dataClient.AddSong(song);
            return Ok(song);
        }
    }
}