using System;
using System.Collections.Generic;
using System.Data.Entity; // Entity Framework 6 - Version-specific usage is crucial!
using System.Linq;

namespace TodoServiceLibrary
{
    //  Dummy classes to represent database entities (simplified)
    public class Todo
    {
        public int TodoId { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TodoContext : DbContext // Inherit from DbContext (EF6)
    {
        //  Explicitly using Entity Framework 6 (v4.3.1 in this case)
        //  This is important for demonstrating migration issues to EF Core
        public TodoContext() : base("name=TodoContext") // Connection string name
        {
        }

        public DbSet<Todo> Todos { get; set; }
    }

    public class DeprecatedEntityFrameworkDependency
    {
        //  This class is ONLY for demonstrating Entity Framework 6 (v4.3.1) usage
        //  and the challenges of migrating to Entity Framework Core in .NET 8.
        //  It should NOT be used in production code.

        private static void EnsureDatabaseExists()
        {
            using (var context = new TodoContext())
            {
                //  Simple check to create the database if it doesn't exist
                //  Adjust as needed for your database setup
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                }
            }
        }

        public static List<Todo> GetTodosFromDatabase()
        {
            EnsureDatabaseExists(); // Ensure the database exists for the demo

            try
            {
                using (var context = new TodoContext())
                {
                    return context.Todos.ToList(); // Simple EF6 query
                }
            }
            catch (Exception ex)
            {
                //  Log the exception (replace with your logging mechanism)
                Console.WriteLine($"Error in GetTodosFromDatabase: {ex.Message}");
                return new List<Todo>(); // Or handle the error as appropriate for your app
            }
        }

        public static void AddTodoToDatabase(string title)
        {
            EnsureDatabaseExists(); // Ensure the database exists for the demo

            try
            {
                using (var context = new TodoContext())
                {
                    context.Todos.Add(new Todo { Title = title, IsCompleted = false });
                    context.SaveChanges(); // Save changes to the database
                }
            }
            catch (Exception ex)
            {
                //  Log the exception (replace with your logging mechanism)
                Console.WriteLine($"Error in AddTodoToDatabase: {ex.Message}");
            }
        }

        public static void DemonstrateEntityFramework()
        {
            Console.WriteLine("Demonstrating Entity Framework 4.3.1 usage...");

            //  Example usage (you might adapt this for your needs)
            AddTodoToDatabase("Example Todo from Deprecated Code");
            List<Todo> todos = GetTodosFromDatabase();

            if (todos != null)
            {
                foreach (var todo in todos)
                {
                    Console.WriteLine($"  - {todo.TodoId}: {todo.Title} (Completed: {todo.IsCompleted})");
                }
            }
            else
            {
                Console.WriteLine("  No Todos found.");
            }
        }
    }
}