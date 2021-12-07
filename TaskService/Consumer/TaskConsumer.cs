using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaskService.Database;

namespace TaskService.Consumer
{
    public class TaskConsumer : IConsumer<TaskMsg>
    {
        private readonly DatabaseContext _context;
        public TaskConsumer(DatabaseContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<TaskMsg> context)
        {
            if (context.Message == null)
            {
                return;
            }
            await Task.Run(() => context.Message);

            string text = "\n Consumed message from the message queue ->  ( " +
                " Emp ID: "  + context.Message.EmpId +
                " Task ID: " + context.Message.TaskId +
                " Start Time: " + context.Message.StartTime.ToString() +")";

            await File.AppendAllTextAsync("log.txt", text);

            try
            {
                TasksDb taskData = null;

                if (context.Message.TaskId>0)
                {
                    taskData = _context.Tasks
                        .Where(x => x.TaskId == context.Message.TaskId).FirstOrDefault();
                }

                if (taskData == null)
                {
                    taskData = new TasksDb();
                }

                taskData.EndTime = context.Message?.EndTime;
                taskData.Status = ( context.Message.Status>0 ) ? context.Message.Status:0;

                //put
                if (taskData.TaskId > 0)
                {
                    //_context.Entry(taskData).State = EntityState.Modified;
                    _context.Tasks.Update(taskData);
                }
                else
                {
                    //post
                    _context.Tasks.Add(taskData);
                }
                
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
               throw; 
            }
        }
    }
}
