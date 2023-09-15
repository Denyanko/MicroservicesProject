using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentServices.Features.Commands.StudentCommands;
using StudentServices.Features.Queries;

namespace StudentServices.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var query = new GetAllStudentsQuery();
            var students = await _mediator.Send(query);

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var query = new GetStudentByIdQuery { StudentId = id };
            var student = await _mediator.Send(query);

            if (student == null) return NotFound();

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentCommand command)
        {
            var studentId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetStudent), new {id = studentId}, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentCommand command)
        {
            if (id != command.StudentId) return BadRequest();

            var result = await _mediator.Send(command);

            if (!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var command = new DeleteStudentCommand { StudentId = id };
            var result = await _mediator.Send(command);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}
