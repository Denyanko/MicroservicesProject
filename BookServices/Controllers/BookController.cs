using BookServices.Features.Commands.BookCommands;
using BookServices.Features.Queries.BookQueries;
using BookServices.Features.Queries.GenreQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookServices.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            var books = await _mediator.Send(query);

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var query = new GetBookByIdQuery { Id = id };
            var book = await _mediator.Send(query);

            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookCommand command)
        {
            var bookId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetBook), new { id = bookId}, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] UpdateBookCommand command)
        {
            if(id != command.Id) return BadRequest();

            var result = await _mediator.Send(command);

            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var command = new DeleteBookCommand { Id = id };
            var result = await _mediator.Send(command);

            if(!result) return NotFound();

            return NoContent();
        }
    }
}
