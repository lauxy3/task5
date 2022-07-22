using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApi.Models;

using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

namespace TaskApi.Controllers
{
    [Route("task")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        //private readonly TaskDBContext _context;
        private readonly IConfiguration _env;

        public TasksController(IConfiguration env)
           
        {
            //_context = context;
            _env = env;

        }

       

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public void PostTask(Models.Task task)
        {
            task.TaskStatus = "STARTED";
            // await _context.SaveChangesAsync();

            var factory = new ConnectionFactory()
            {
                HostName = _env.GetSection("RABBITMQ_HOST").Value,
                Port = Convert.ToInt32(_env.GetSection("RABBITMQ_PORT").Value),
                UserName = _env.GetSection("RABBITMQ_USER").Value,
                Password = _env.GetSection("RABBITMQ_PASSWORD").Value
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "tasks", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string message = string.Empty;
                message = JsonSerializer.Serialize(task);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: "tasks", basicProperties: null, body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

        }
    }

    
    
}
