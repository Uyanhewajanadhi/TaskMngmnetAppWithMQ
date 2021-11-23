using MassTransit;
using SharedModels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.Consumer
{
    public class TaskConsumer : IConsumer<TaskMsg>
    {
        public async Task Consume(ConsumeContext<TaskMsg> context)
        {
            var data = context.Message;
        }
    }
}
