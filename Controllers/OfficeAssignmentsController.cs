﻿using System;
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
    public class OfficeAssignmentsController : ControllerBase
    {
        private readonly ContosouniversityContext _context;

        public OfficeAssignmentsController(ContosouniversityContext context)
        {
            _context = context;
        }

        // GET: api/OfficeAssignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfficeAssignment>>> GetOfficeAssignmentAsync()
        {
            return await _context.OfficeAssignment.ToListAsync();
        }

        // GET: api/OfficeAssignments/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OfficeAssignment>> GetOfficeAssignmentAsync(int id)
        {
            var officeAssignment = await _context.OfficeAssignment.FindAsync(id);

            if (officeAssignment == null)
            {
                return NotFound();
            }

            return officeAssignment;
        }

        // PUT: api/OfficeAssignments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> PutOfficeAssignmentAsync(int id, OfficeAssignment officeAssignment)
        {
            if (id != officeAssignment.InstructorId)
            {
                return BadRequest();
            }

            _context.Entry(officeAssignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OfficeAssignmentExists(id))
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

        // POST: api/OfficeAssignments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OfficeAssignment>> PostOfficeAssignmentAsync(OfficeAssignment officeAssignment)
        {
            await _context.OfficeAssignment.AddAsync(officeAssignment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (OfficeAssignmentExists(officeAssignment.InstructorId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetOfficeAssignmentAsync), new { id = officeAssignment.InstructorId }, officeAssignment);
        }

        // DELETE: api/OfficeAssignments/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<OfficeAssignment>> DeleteOfficeAssignmentAsync(int id)
        {
            var officeAssignment = await _context.OfficeAssignment.FindAsync(id);
            if (officeAssignment == null)
            {
                return NotFound();
            }

            _context.OfficeAssignment.Remove(officeAssignment);
            await _context.SaveChangesAsync();

            return officeAssignment;
        }

        private bool OfficeAssignmentExists(int id)
        {
            return _context.OfficeAssignment.Any(e => e.InstructorId == id);
        }
    }
}
