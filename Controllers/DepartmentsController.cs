﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspcore3hw.models
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public DepartmentsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartmentAsync()
        {
            return await _context.Department.Where(e => !e.IsDeleted).ToListAsync();
        }
        [HttpGet("{DepartmentCourseCount}")]
        public async Task<ActionResult<IEnumerable<VwDepartmentCourseCount>>> GetVwDepartmentCourseCountAsync(int id)
        {
            return await _context.VwDepartmentCourseCount.FromSqlInterpolated($"SELECT * FROM vwDepartmentCourseCount").ToListAsync();
        }
        
        // GET: api/Departments/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Department>> GetDepartmentAsync(int id)
        {
            var department = await _context.Department.FirstOrDefaultAsync(e => e.DepartmentId.Equals(id) && !e.IsDeleted);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutDepartmentAsync(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest();
            }

            //預存程序
            var count = await _context.Database.ExecuteSqlInterpolatedAsync($"EXECUTE Department_Update {department.DepartmentId},{department.Name},{department.Budget},{department.StartDate},{department.InstructorId},{department.RowVersion}");

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Department>> PostDepartmentAsync(Department department)
        {
            if( !ModelState.IsValid)
            {
                return BadRequest();
            }
            //預存程序
            var sp_department  = await(
                                 from a in _context.Department.FromSqlInterpolated($"EXECUTE Department_Insert {department.Name},{department.Budget},{department.StartDate},{department.InstructorId}")
                                 select new { a.DepartmentId}
                                 ).ToListAsync();

            return CreatedAtAction(nameof(GetDepartmentAsync), new { id = department.DepartmentId }, sp_department);
        }

        // DELETE: api/5/0x00000000000007D4
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartmentAsync(int id, Department department)
        {
            if (!DepartmentVersionExists(id,department.RowVersion))
            {
                return NotFound();
            }    
            //直接執行sql語句(預存程序)
            var count = await _context.Database.ExecuteSqlInterpolatedAsync($"EXECUTE Department_Delete {id},{department.RowVersion}");

            return new JsonResult(count);
        }
        [HttpDelete("{id}/tag")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Department>> TagDepartmentAsync(int id)
        {
            var department = await _context.Department.SingleOrDefaultAsync(e => e.DepartmentId.Equals(id) && !e.IsDeleted);
            if (department == null)
            {
                return NotFound();
            }
            //註記刪除標誌
            department.IsDeleted = true;

            await _context.SaveChangesAsync();

            return department;
        }
        private bool DepartmentVersionExists(int id,byte[] version)
        {
            return _context.Department.Any(e => e.DepartmentId == id && e.RowVersion == version && !e.IsDeleted);
        }
        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(e => e.DepartmentId == id);
        }
    }
}
