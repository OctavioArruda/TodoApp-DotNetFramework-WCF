using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TodoServiceLibrary;
using Newtonsoft.Json; // Add Newtonsoft.Json
using NSubstitute;

namespace TodoServiceLibrary.Tests
{
    [TestClass]
    public class TodoServiceTestsWithIncompatibility
    {
        private ITodoService _mockTodoService;

        [TestInitialize]
        public void Setup()
        {
            _mockTodoService = Substitute.For<ITodoService>();
        }

        [TestMethod]
        public void GetTodoItem_WithValidId_ReturnsTodoItem()
        {
            // Arrange
            int validId = 1;
            TodoItem expectedItem = new TodoItem { Id = validId, Title = "Test Item", IsCompleted = false };
            _mockTodoService.GetTodoItem(validId).Returns(expectedItem);

            // Act
            TodoItem result = _mockTodoService.GetTodoItem(validId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedItem.Title, result.Title);
            Assert.AreEqual(expectedItem.Id, result.Id);

            // Demonstrate Newtonsoft.Json usage (for potential migration issues)
            string json = JsonConvert.SerializeObject(expectedItem);
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void GetAllTodoItems_ReturnsListOfTodoItems()
        {
            // Arrange
            var expectedItems = new List<TodoItem>
                    {
                        new TodoItem { Id = 1, Title = "Item 1", IsCompleted = false },
                        new TodoItem { Id = 2, Title = "Item 2", IsCompleted = true }
                    };
            _mockTodoService.GetAllTodoItems().Returns(expectedItems);

            // Act
            List<TodoItem> result = _mockTodoService.GetAllTodoItems();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            // Demonstrate Newtonsoft.Json usage (for potential migration issues)
            string json = JsonConvert.SerializeObject(expectedItems);
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public void AddTodoItem_AddsItem()
        {
            // Arrange
            TodoItem newItem = new TodoItem { Title = "New Item", IsCompleted = false };
            _mockTodoService.AddTodoItem(Arg.Any<TodoItem>());

            // Act
            //_service.AddTodoItem(newItem);  // Pretend this is a class that uses the service

            // Assert
            _mockTodoService.Received(1).AddTodoItem(Arg.Is<TodoItem>(arg => arg.Title == "New Item"));

            // Demonstrate Newtonsoft.Json usage (for potential migration issues)
            string json = JsonConvert.SerializeObject(newItem);
            Assert.IsNotNull(json);
        }
    }
}