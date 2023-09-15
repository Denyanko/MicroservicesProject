using BookServices.Features.Commands.AuthorCommands;
using BookServices.Features.Queries.AuthorQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookServices.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var query = new GetAllAuthorsQuery();
            var authors = await _mediator.Send(query);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var query = new GetAuthorByIdQuery { Id = id };
            var author = await _mediator.Send(query);

            if (author == null) return NotFound();

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorCommand command)
        {
            var authorId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAuthor), new {id = authorId}, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] UpdateAuthorCommand command)
        {
            if (id != command.Id) return BadRequest();

            var result = await _mediator.Send(command);

            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var command = new DeleteAuthorCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}
