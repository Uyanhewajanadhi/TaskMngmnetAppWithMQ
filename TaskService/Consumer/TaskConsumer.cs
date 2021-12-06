using MassTransit;
using SharedModels.Models;
using System.IO;
using System.Threading.Tasks;

namespace TaskService.Consumer
{
    public class TaskConsumer : IConsumer<TaskMsg>
    {
        public async Task Consume(ConsumeContext<TaskMsg> context)
        {           
            await Task.Run(() => context.Message);

            string text = "\n Consumed message from the message queue ->  ( " +
                " Emp ID: "  + context.Message.EmpId +
                " Task ID: " + context.Message.TaskId +
                " Start Time: " + context.Message.startTime.ToString() +")";

            await File.AppendAllTextAsync("log.txt", text);
        }
    }
}
