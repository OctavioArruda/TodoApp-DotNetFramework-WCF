using System;
using System.Collections.Generic;
using TodoServiceLibrary;

namespace TodoServiceLibraryTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TodoService service = new TodoService(); // Instantiate the service

            // Test operations
            TodoItem newItem = new TodoItem { Title = "Test Todo", IsCompleted = false };
            service.AddTodoItem(newItem);

            List<TodoItem> allItems = service.GetAllTodoItems();
            foreach (var item in allItems)
            {
                Console.WriteLine($"ID: {item.Id}, Title: {item.Title}, Completed: {item.IsCompleted}");
            }

            TodoItem itemToUpdate = service.GetTodoItem(1);
            if (itemToUpdate != null)
            {
                itemToUpdate.IsCompleted = true;
                service.UpdateTodoItem(itemToUpdate);
            }

            service.DeleteTodoItem(2);

            Console.WriteLine("\nAfter Updates:");
            allItems = service.GetAllTodoItems();
            foreach (var item in allItems)
            {
                Console.WriteLine($"ID: {item.Id}, Title: {item.Title}, Completed: {item.IsCompleted}");
            }

            Console.ReadKey();
        }
    }
}