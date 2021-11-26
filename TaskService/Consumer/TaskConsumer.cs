using MassTransit;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskService.Contracts;

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
