using APID3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APID3.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        //public SchoolContext _context = new SchoolContext(); xxxxxxxxxx
        private readonly SchoolContext _context;
        public StudentController(SchoolContext _context)
        {
            this._context = _context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (_context.Students == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "No students found");
            }

            return Ok(_context.Students.ToList());
        }

        [HttpGet("{Id:int}")]
        public IActionResult GetById(int Id)
        {
            var student = _context.Students.FirstOrDefault(s => s.ID == Id);
            if (student == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Student not found");
            }
            return Ok(student);
        }

        // Edit the student
        [HttpPut("{Id:int}")]
        public IActionResult EditStudent(int Id, [FromBody] Student s)
        {
            if (s == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Student not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the student, if not found return 404 , if found update the student
            _context.Entry(s).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // Add A new student
        [HttpPost]
        public IActionResult AddStudent(Student s)
        {

            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Student not found");
            }
            _context.Students.Add(s);
            _context.SaveChanges();

            return CreatedAtAction("GetById", new { s.ID }, s);
        }
    }
}
