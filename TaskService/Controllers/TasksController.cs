using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.DTOs;
using TaskService.Contracts;
using TaskService.Database;
using static SharedModels.Models.Enums;

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

        public List<TasksDb> GetTasksNames()
        {
            var query = from b in _context.Tasks orderby b.TaskId select b;
            return query.ToList();
        }

        public List<TasksDb> GetTasksStatus(int state)
        {
            var query = from b in _context.Tasks where b.Status == state orderby b.TaskId select b;
            return query.ToList();
        }

        public TasksDb GetTasksId(int Task_id)
        {
            var query = from b in _context.Tasks where b.TaskId == Task_id select b;
            return query.FirstOrDefault();
        }

        public List<TasksDb> PostTasks()
        {
            var query = from b in _context.Tasks orderby b.TaskName select b;
            return query.ToList();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TasksDb>> GetTasksDb(int id)
        {
            var tasksDb = await _context.Tasks.FindAsync(id);

            if (tasksDb == null) { return NotFound(); }

            
            return tasksDb;
        }

        // GET: api/Tasks/0
        [HttpGet("statustype/{statusType}")]
        public async Task<ActionResult<List<TasksDb>>> GetTasksForType(TaskStateTypes statusType)
        {

            var tasks = await _context.Tasks
                        .Where(b => b.Status == (int)statusType)
                        .ToListAsync();

            if (tasks == null) { return NotFound(); }
            

            return tasks;
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasksDb(int id, TasksDb tasksDb)
        {

            if (id != tasksDb.TaskId) { return BadRequest(); }


            _context.Entry(tasksDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksDbExists(id)) { return NotFound(); }
                else { throw; }
            }

            return NoContent();
        }

        // POST: api/Tasks
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
            if (tasksDb == null) { return NotFound(); }


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
