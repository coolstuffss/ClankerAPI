using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ClankerAPI.Models;

namespace ClankerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private List<TodoItem> DatabaseRecords { get; set; }

        public TodoController()
        {
            DatabaseRecords = TodoItem.SelectAllRecords();
        }

        [HttpGet]
        [EnableCors("AllowAll")]
        public ActionResult<IEnumerable<TodoItem>> GetTodoItem()
        {
            return DatabaseRecords.ToArray();
        }

        [HttpGet("{id}")]
        [EnableCors("AllowAll")]
        public ActionResult<TodoItem> GetTodoItem(long id)
        {
            TodoItem td = DatabaseRecords.Find(x => x.Id == id);
        
            if (td != null){
                return td;
            }
            return NotFound();
        }

        [HttpPost]
        [EnableCors("AllowAll")]
        public ActionResult<TodoItem> InsertTodoItem(TodoItem td)
        {
            td.Id = 0;
            if (!td.SaveToDb()){
                return BadRequest();
            }
            if (!td.LoadFromDb()){
                return BadRequest();
            }
            DatabaseRecords = TodoItem.SelectAllRecords();
            
            return CreatedAtAction(nameof(GetTodoItem), new { Id = td.Id }, td);
        }

        [HttpPut("{id}")]
        [EnableCors("AllowAll")]
        public ActionResult<TodoItem> UpdateTodoItem(long id, TodoItem td)
        {
            if (id != td.Id){
                return BadRequest();
            }
            if (!td.SaveToDb()){
                return BadRequest();
            }
            DatabaseRecords = TodoItem.SelectAllRecords();
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        [EnableCors("AllowAll")]
        public ActionResult<TodoItem> DeleteTodoItem(long id)
        {
            var td = new TodoItem {Id = id};
            if (!td.DeleteFromDb()){
                return BadRequest();
            }
            DatabaseRecords = TodoItem.SelectAllRecords();
            
            return NoContent();
        }
    }
}