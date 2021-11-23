using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskService.Database;

namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TasksController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TasksDb>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TasksDb>> GetTasksDb(int id)
        {
            var tasksDb = await _context.Tasks.FindAsync(id);

            if (tasksDb == null)
            {
                return NotFound();
            }

            return tasksDb;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasksDb(int id, TasksDb tasksDb)
        {
            if (id != tasksDb.TaskId)
            {
                return BadRequest();
            }

            _context.Entry(tasksDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksDbExists(id))
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

        // POST: api/Tasks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TasksDb>> PostTasksDb(TasksDb tasksDb)
        {
            _context.Tasks.Add(tasksDb);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTasksDb", new { id = tasksDb.TaskId }, tasksDb);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TasksDb>> DeleteTasksDb(int id)
        {
            var tasksDb = await _context.Tasks.FindAsync(id);
            if (tasksDb == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(tasksDb);
            await _context.SaveChangesAsync();

            return tasksDb;
        }

        private bool TasksDbExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
