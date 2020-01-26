using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataBase.Data;
using DataBase.Model;

namespace DataBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly Context _context;

        public BugsController(Context context)
        {
            _context = context;
        }

        // GET: api/Bugs
        [HttpGet]
        public IEnumerable<Bug> GetBugItem()
        {
            return _context.BugItem;
        }

        // GET: api/Bugs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBug([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bug = await _context.BugItem.FindAsync(id);

            if (bug == null)
            {
                return NotFound();
            }

            return Ok(bug);
        }

        // PUT: api/Bugs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBug([FromRoute] Guid id, [FromBody] Bug bug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bug.TaskId)
            {
                return BadRequest();
            }

            _context.Entry(bug).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BugExists(id))
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

        // POST: api/Bugs
        [HttpPost]
        public async Task<IActionResult> PostBug([FromBody] Bug bug)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BugItem.Add(bug);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBug", new { id = bug.TaskId }, bug);
        }

        // DELETE: api/Bugs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBug([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bug = await _context.BugItem.FindAsync(id);
            if (bug == null)
            {
                return NotFound();
            }

            _context.BugItem.Remove(bug);
            await _context.SaveChangesAsync();

            return Ok(bug);
        }

        private bool BugExists(Guid id)
        {
            return _context.BugItem.Any(e => e.TaskId == id);
        }
    }
}