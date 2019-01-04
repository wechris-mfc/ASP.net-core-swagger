using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (!_context.TodoItems.Any())
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }


        /// <summary>
        /// Gets All TodoItems.
        /// </summary>
        /// <returns>A List of todo Items </returns>
        /// <response code="200">Returns TodoItems as Json Object </response>
        /// <response code="400">If the item is null</response>  
        // GET: api/Todo
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }


        /// <summary>
        /// Get a single TodoItem.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>get a single TodoItem</returns>
        /// <response code="200">Returns the todoItem as json</response>
        /// <response code="404">If the item is null</response>
        // GET: api/Todo/1 
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound("Item not found");
            }

            return todoItem;
        }



        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="todoItem"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>            
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            await _context.TodoItems.AddAsync(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);

        }

        ///  <summary>
        ///  Updates a todoItem.
        ///  </summary>
        ///  <remarks>
        ///  Sample request:
        /// 
        ///      PUT /Todo/1
        ///      {
        ///         "id": 1,
        ///         "name": "Item1",
        ///         "isComplete": true
        ///      }
        /// 
        ///  </remarks>
        /// <param name="id"></param>
        /// <param name="todoItem"></param>
        ///  <returns>A newly created TodoItem</returns>
        ///  <response code="204">Success - No Content Returned</response>
        ///  <response code="404">If the item is not found</response>            

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        ///  <summary>
        ///  Deletes a todoItem.
        ///  </summary>
        /// <param name="id"></param>
        ///  <returns>A newly created TodoItem</returns>
        ///  <response code="200">Success - Deleted json object</response>
        ///  <response code="404">If the item is not found</response>  
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }
    }
}
