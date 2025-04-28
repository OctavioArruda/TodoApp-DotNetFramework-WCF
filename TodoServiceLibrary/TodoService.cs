using System.Collections.Generic;
using System.Linq;

namespace TodoServiceLibrary
{
    /*
    * "TodoService" class implements the "ITodoService" interface.
    * This class contains the actual logic for each service operation.
    * For simplicity, an in-memory "_todoItems" is being use
    * */
    public class TodoService : ITodoService
    {
        private static List<TodoItem> _todoItems = new List<TodoItem>()
            {
                new TodoItem { Id = 1, Title = "Learn WCF", IsCompleted = true },
                new TodoItem { Id = 2, Title = "Build Todo App", IsCompleted = false }
            };

        public TodoItem GetTodoItem(int id)
        {
            return _todoItems.FirstOrDefault(item => item.Id == id);
        }

        public List<TodoItem> GetAllTodoItems()
        {
            return _todoItems;
        }

        public void AddTodoItem(TodoItem item)
        {
            item.Id = _todoItems.Count + 1; // Simple ID generation
            _todoItems.Add(item);
        }

        public void UpdateTodoItem(TodoItem item)
        {
            var existingItem = _todoItems.FirstOrDefault(i => i.Id == item.Id);
            if (existingItem != null)
            {
                existingItem.Title = item.Title;
                existingItem.IsCompleted = item.IsCompleted;
            }
        }

        public void DeleteTodoItem(int id)
        {
            _todoItems.RemoveAll(item => item.Id == id);
        }
    }
}