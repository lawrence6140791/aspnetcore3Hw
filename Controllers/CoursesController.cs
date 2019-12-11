using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aspcore3hw.models;

namespace aspcore3hw
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public CoursesController(ContosouniversityContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourseAsync()
        {
            return await _context.Course.Where(e => !e.IsDeleted).ToListAsync();
        }
        // GET: api/Courses/students/count
        [HttpGet("{students}/{count}")]
        public async Task<ActionResult<IEnumerable<VwCourseStudentCount>>> GetVwCourseStudentCountAsync()
        {
            return await _context.VwCourseStudentCount.ToListAsync();
        }
        // GET: api/Courses/students
        [HttpGet("{students}")]
        public async Task<ActionResult<IEnumerable<VwCourseStudents>>> GetVwCourseStudentsAsync()
        {
            return await _context.VwCourseStudents.ToListAsync();
        }
        // GET: api/Courses/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Course>> GetCourseAsync(int id)
        {
            var course = await _context.Course.FirstOrDefaultAsync(e => e.CourseId.Equals(id) && !e.IsDeleted);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }
        // PUT: api/Courses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourseAsync(int id, Course course)
        {
            if (id != course.CourseId)
            {
                return BadRequest();
            }

            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourseAsync(Course course)
        {
            if( !ModelState.IsValid)
            {
                return BadRequest();
            }
            
            if (course.CourseId.Equals(0)) 
            {
                await _context.Course.AddAsync(course);
                await _context.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }           

            return CreatedAtAction("GetCourse", new { id = course.CourseId }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Course>> DeleteCourseAsync(int id)
        {

            var course = await _context.Course.FirstOrDefaultAsync(e => e.CourseId.Equals(id) && !e.IsDeleted);

            if (course == null)
            {
                return NotFound();
            }

            course.IsDeleted = true;

            await _context.SaveChangesAsync();

            return course;
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
