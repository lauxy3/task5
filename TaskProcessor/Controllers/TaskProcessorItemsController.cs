using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using TaskProcessor.Models;

namespace TaskProcessor.Controllers
{
    [Route("task-processor:2.0")]
    [ApiController]
    public class TaskProcessorItemsController : ControllerBase
    {
        private readonly TaskProDBContext _context;

        public TaskProcessorItemsController(TaskProDBContext context)
        {
            _context = context;
        }

        // GET: api/TaskProcessorItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskProcessorItem>>> GetTaskProcessorItems()
        {
            System.Diagnostics.Debug.WriteLine("Consumer consume");
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "tasks",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                return await _context.TaskProcessItems.ToListAsync();
            }
        }
        // GET: api/TaskProcessorItems/5
        [HttpGet("{id}")]
            public async Task<ActionResult<TaskProcessorItem>> GetTaskProcessorItem(int id)
        {
            var taskProcessorItem = await _context.TaskProcessItems.FindAsync(id);

            if (taskProcessorItem == null)
            {
                return NotFound();
            }

            return taskProcessorItem;
        }

        // PUT: api/TaskProcessorItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskProcessorItem(int id, TaskProcessorItem taskProcessorItem)
        {
            if (id != taskProcessorItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskProcessorItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskProcessorItemExists(id))
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

        // POST: api/TaskProcessorItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskProcessorItem>> PostTaskProcessorItem(TaskProcessorItem taskProcessorItem)
        {
            _context.TaskProcessItems.Add(taskProcessorItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskProcessorItem", new { id = taskProcessorItem.Id }, taskProcessorItem);
        }

        // DELETE: api/TaskProcessorItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskProcessorItem(int id)
        {
            var taskProcessorItem = await _context.TaskProcessItems.FindAsync(id);
            if (taskProcessorItem == null)
            {
                return NotFound();
            }

            _context.TaskProcessItems.Remove(taskProcessorItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskProcessorItemExists(int id)
        {
            return _context.TaskProcessItems.Any(e => e.Id == id);
        }
    }
}
