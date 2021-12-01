using MassTransit;
using SharedModels.Models;
using System.Threading.Tasks;

namespace TaskService.Consumer
{
    public class TaskConsumer : IConsumer<TaskMsg>
    {
        public async Task Consume(ConsumeContext<TaskMsg> context)
        {
            var data = context.Message;

            //write to a file

        }
    }
}
