﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskService.Contracts;
using TaskService.Database;

namespace TaskService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ILoggerService _logger;

        private readonly DatabaseContext _context;

        public TasksController(DatabaseContext context, ILoggerService logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TasksDb>>> GetTasks()
        {
            _logger.LogInfo("User data has been retrieved");
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

            string msg = "User data with id" + id + "has been retrieved";
            _logger.LogInfo(msg);

            return tasksDb;
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasksDb(int id, TasksDb tasksDb)
        {

            //get the password and check if it with the password in User Table.
            //even the password is needed to be changed, check it  with the previous password.

            if (id != tasksDb.TaskId)
            {
                return BadRequest();
            }

            string msg = "User data with id" + id + "has been updated";
            _logger.LogInfo(msg);

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
        [HttpPost]
        public async Task<ActionResult<TasksDb>> PostTasksDb(TasksDb tasksDb)
        {

            //get the password from the user id and check it matches.
            //var user =  _context.User.FindAsync(userID);
            //var password = _context.User.Where(x => x.EmpId == 1).Select(x => x.Password).FirstOrDefault();

            //  //incoming pw
            //  var hashedPassword = PasswordManager.HashPassword(tasks.password);

            //  if (hashedPassword == password)
            //  {
            //      true
            //  }

            _logger.LogInfo("New data has been created");

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

            string msg = "User data with id" + id + "has been deleted";
            _logger.LogWarn(msg);


            _context.Tasks.Remove(tasksDb);
            await _context.SaveChangesAsync();

            //get the password from the user who's trying to delete the record
            //and check if the password matches for the password in User table

            return tasksDb;
        }

        private bool TasksDbExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
