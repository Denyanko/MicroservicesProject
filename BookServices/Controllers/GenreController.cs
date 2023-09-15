using BookServices.Features.Commands.GenreCommands;
using BookServices.Features.Queries.GenreQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookServices.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var query = new GetAllGenresQuery();
            var genres = await _mediator.Send(query);

            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenre(int id)
        {
            var query = new GetGenreByIdQuery { Id = id };
            var genre = await _mediator.Send(query);

            if (genre == null) return NotFound();

            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreCommand command)
        {
            var genreId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetGenre), new {id = genreId}, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] UpdateGenreCommand command)
        {
            if(id != command.Id) return BadRequest();

            var result = await _mediator.Send(command);

            if(!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var command = new DeleteGenreCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}
