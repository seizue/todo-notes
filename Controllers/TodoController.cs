using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Data;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.OrderBy(t => t.Order).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null) return NotFound();
            return todoItem;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(CreateTodoItemDto dto)
        {
            var item = new TodoItem
            {
                Title = dto.Title,
                IsComplete = dto.IsComplete,
                DueDate = dto.DueDate,
                Tags = dto.Tags,
                Priority = dto.Priority,
                Order = await _context.TodoItems.CountAsync()
            };

            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItem updated)
        {
            if (id != updated.Id) return BadRequest();

            _context.Entry(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null) return NotFound();

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("reorder")]
        public async Task<IActionResult> Reorder([FromBody] List<long> orderedIds)
        {
            var items = await _context.TodoItems.ToListAsync();

            for (int i = 0; i < orderedIds.Count; i++)
            {
                var item = items.FirstOrDefault(x => x.Id == orderedIds[i]);
                if (item != null)
                {
                    item.Order = i;
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
