using BookServices.Features.Commands.BorrowingCommands;
using BookServices.Features.Queries.BorrowQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookServices.Controllers
{
    [Route("api/borrow-books")]
    [ApiController]
    public class BorrowBookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BorrowBookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowBooks(int? BookId, int? StudentId)
        {
            var query = new BorrowBookQueries
            {
                BookId = BookId,
                StudentId = StudentId
            };

            var borrowBooks = await _mediator.Send(query);

            return Ok(borrowBooks);
        }

        [HttpPost]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookCommand command)
        {
            var borrow = await _mediator.Send(command);

            if (borrow == null) return Ok("You have not returned this book yet");

            return Ok(borrow);
        }

        [HttpPut]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result) return Ok("You have not returned this book yet");

            return NoContent();
        }
    }
}
