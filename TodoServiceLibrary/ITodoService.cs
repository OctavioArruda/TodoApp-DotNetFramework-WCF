using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

/* Define a simple "TodoItem" class to represent data.
 * "DataContract" and "DataMember" attributes are essential for WCF to serialize and deserialize the data.
 * "ITodoService" interface declares the operations our service will support (CRUD operations for a Todo list).
 * "ServiceContract" and "OperationalContract" attributed are mandadoty for WCF
 * */
namespace TodoServiceLibrary
{
    // Data Transfer Object (DTO) - For passing Todo items
    [DataContract] // Important for WCF serialization
    public class TodoItem
    {
        [DataMember] // Mark each property to be serialized
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public bool IsCompleted { get; set; }
    }

    [ServiceContract] // This interface defines the service contract
    public interface ITodoService
    {
        [OperationContract] // Each method is a service operation
        TodoItem GetTodoItem(int id);

        [OperationContract]
        List<TodoItem> GetAllTodoItems();

        [OperationContract]
        void AddTodoItem(TodoItem item);

        [OperationContract]
        void UpdateTodoItem(TodoItem item);

        [OperationContract]
        void DeleteTodoItem(int id);
    }
}