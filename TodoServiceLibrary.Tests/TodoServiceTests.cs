using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TodoServiceLibrary.Tests
{
    [TestClass]
    public class TodoServiceTests
    {
        private TodoService _service;
        private TodoItem _testItem1;
        private TodoItem _testItem2;

        [TestInitialize]
        public void TestInitialize()
        {
            _service = new TodoService(new DeprecatedDependencyInjectionDemo());
            // Initialize with known data for consistent testing
            _service.GetAllTodoItems().Clear(); // Clear existing data
            _testItem1 = new TodoItem { Id = 1, Title = "Initial Item 1", IsCompleted = false };
            _testItem2 = new TodoItem { Id = 2, Title = "Initial Item 2", IsCompleted = true };
            _service.AddTodoItem(_testItem1);
            _service.AddTodoItem(_testItem2);
        }

        [TestMethod]
        public void GetAllTodoItems_ReturnsAllItems()
        {
            // Act
            List<TodoItem> items = _service.GetAllTodoItems();

            // Assert
            Assert.IsNotNull(items);
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual("Initial Item 1", items[0].Title);
            Assert.AreEqual("Initial Item 2", items[1].Title);
        }

        [TestMethod]
        public void GetTodoItem_WithValidId_ReturnsCorrectItem()
        {
            // Act
            TodoItem item = _service.GetTodoItem(1);

            // Assert
            Assert.IsNotNull(item);
            Assert.AreEqual("Initial Item 1", item.Title);
            Assert.AreEqual(1, item.Id);
        }

        [TestMethod]
        public void GetTodoItem_WithInvalidId_ReturnsNull()
        {
            // Act
            TodoItem item = _service.GetTodoItem(999);

            // Assert
            Assert.IsNull(item);
        }

        [TestMethod]
        public void AddTodoItem_AddsNewItemAndAssignsId()
        {
            // Arrange
            TodoItem newItem = new TodoItem { Title = "Buy groceries", IsCompleted = false };

            // Act
            _service.AddTodoItem(newItem);
            List<TodoItem> items = _service.GetAllTodoItems();
            TodoItem addedItem = items.Find(item => item.Title == "Buy groceries");

            // Assert
            Assert.IsNotNull(addedItem);
            Assert.AreEqual(3, addedItem.Id); // Check for correct ID assignment
            Assert.IsFalse(addedItem.IsCompleted);
        }

        [TestMethod]
        public void UpdateTodoItem_WithValidId_UpdatesItem()
        {
            // Arrange
            TodoItem updatedItem = new TodoItem { Id = 1, Title = "Updated Item", IsCompleted = true };

            // Act
            _service.UpdateTodoItem(updatedItem);
            TodoItem retrievedItem = _service.GetTodoItem(1);

            // Assert
            Assert.IsNotNull(retrievedItem);
            Assert.AreEqual("Updated Item", retrievedItem.Title);
            Assert.IsTrue(retrievedItem.IsCompleted);
        }

        [TestMethod]
        public void UpdateTodoItem_WithInvalidId_DoesNotUpdateAnything()
        {
            // Arrange
            TodoItem updatedItem = new TodoItem { Id = 999, Title = "Nonexistent", IsCompleted = true };
            List<TodoItem> initialItems = _service.GetAllTodoItems(); // Capture initial state

            // Act
            _service.UpdateTodoItem(updatedItem);
            List<TodoItem> finalItems = _service.GetAllTodoItems();

            // Assert
            Assert.AreEqual(initialItems.Count, finalItems.Count); // Ensure no items were added/removed
            TodoItem originalItem = _service.GetTodoItem(1);
            Assert.AreEqual("Initial Item 1", originalItem.Title); // Ensure original item is unchanged
        }

        [TestMethod]
        public void DeleteTodoItem_WithValidId_DeletesItem()
        {
            // Act
            _service.DeleteTodoItem(1);
            TodoItem deletedItem = _service.GetTodoItem(1);
            List<TodoItem> items = _service.GetAllTodoItems();

            // Assert
            Assert.IsNull(deletedItem);
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual("Initial Item 2", items[0].Title);
        }

        [TestMethod]
        public void DeleteTodoItem_WithInvalidId_DoesNotThrowException()
        {
            // Act & Assert (in this case, we're checking for no exception)
            Assert.IsTrue(true); // Placeholder, test might need adjustment depending on your implementation
            _service.DeleteTodoItem(999); // Should not throw an exception
        }
    }
}