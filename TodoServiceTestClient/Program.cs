using System;
using System.Collections.Generic;
// REMOVE the direct reference to TodoServiceLibrary for service implementation:
// using TodoServiceLibrary;

// ADD the using directive for the generated WCF client proxy namespace:
// Check your Solution Explorer under TodoServiceTestClient -> Service References
// The namespace is usually ProjectName.ServiceReferencesFolderName
using TodoServiceTestClient.ServiceReference1; // <-- ADJUST THIS NAMESPACE if needed!

// ADD using for WCF client communication states
using System.ServiceModel;
using System.Linq;
using TodoServiceLibrary;


namespace TodoServiceLibraryTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // --- Endpoint Selection ---
            // Set this boolean to true to use the NetTcp endpoint,
            // set to false to use the HTTP endpoint (assuming one is configured).
            bool useNetTcp = true; // <-- CHANGE THIS TO SWITCH ENDPOINTS

            // Define the endpoint names as they appear in TodoServiceTestClient\App.config
            // You confirmed the NetTcp name is "NetTcpBinding_ITodoService".
            const string netTcpEndpointName = "NetTcpBinding_ITodoService";

            // You need to check your App.config for the name of your HTTP endpoint
            // (the one pointing to the .svc file in TodoServiceWeb).
            // It's likely something like "BasicHttpBinding_ITodoService" or similar.
            // Replace "Your_Actual_HTTP_Endpoint_Name_Here" with that name.
            const string httpEndpointName = "BasicHttpBinding_ITodoService"; // <-- *** IMPORTANT: FIND THIS IN YOUR App.config ***

            string endpointName = useNetTcp ? netTcpEndpointName : httpEndpointName;

            Console.WriteLine($"Attempting to connect using endpoint: {endpointName}");
            // --- End Endpoint Selection ---


            // --- Using the WCF Client Proxy ---
            // Instead of directly instantiating the service, we instantiate
            // the generated client proxy class (TodoServiceClient).
            // The proxy reads the endpoint configuration from App.config based on the name provided.
            // The 'using' block ensures the client channel is properly closed or aborted.
            using (TodoServiceClient client = new TodoServiceClient(endpointName))
            {
                try
                {
                    Console.WriteLine("Client proxy created. Calling service operations...");

                    // Test operations - NOW calling the service REMOTELY via the proxy
                    // Note: Data contract types (like TodoItem) are also in the ServiceReference1 namespace.
                    // You might need to fully qualify them (e.g., ServiceReference1.TodoItem)
                    // or add a 'using' alias if there are naming conflicts.
                    TodoItem newItem = new TodoItem { Title = "Test Todo via WCF", IsCompleted = false };
                    client.AddTodoItem(newItem); // Calling the remote WCF method
                    Console.WriteLine("Added new item via WCF.");

                    Console.WriteLine("\nGetting all items via WCF...");
                    List<TodoItem> allItems = client.GetAllTodoItems().ToList(); // Need .ToList() if service returns array
                    foreach (var item in allItems)
                    {
                        Console.WriteLine($"ID: {item.Id}, Title: {item.Title}, Completed: {item.IsCompleted}");
                    }

                    Console.WriteLine("\nUpdating item 1 via WCF...");
                    TodoItem itemToUpdate = client.GetTodoItem(1);
                    if (itemToUpdate != null)
                    {
                        itemToUpdate.IsCompleted = true;
                        client.UpdateTodoItem(itemToUpdate); // Calling the remote WCF method
                        Console.WriteLine("Item 1 updated.");
                    }
                    else
                    {
                        Console.WriteLine("Item 1 not found.");
                    }

                    Console.WriteLine("\nDeleting item 2 via WCF...");
                    client.DeleteTodoItem(2); // Calling the remote WCF method
                    Console.WriteLine("Item 2 deleted.");


                    Console.WriteLine("\nAfter Updates (via WCF):");
                    allItems = client.GetAllTodoItems().ToList();
                    foreach (var item in allItems)
                    {
                        Console.WriteLine($"ID: {item.Id}, Title: {item.Title}, Completed: {item.IsCompleted}");
                    }

                    // Note: The DeprecatedDependencyInjectionDemo logic runs on the SERVER side now!


                    // --- WCF Client Clean Up ---
                    // It's crucial to close the client channel properly.
                    // The 'using' statement helps, but manual checking is safer with WCF.
                    if (client.State != CommunicationState.Faulted)
                    {
                        client.Close();
                        Console.WriteLine("Client channel closed successfully.");
                    }
                }
                catch (TimeoutException timeoutEx)
                {
                    Console.WriteLine($"The operation timed out: {timeoutEx.Message}");
                    // Abort the channel if it faulted
                    if (client.State != CommunicationState.Closed)
                    {
                        client.Abort();
                        Console.WriteLine("Client channel aborted due to timeout.");
                    }
                }
                catch (FaultException<Exception> faultEx) // Catch specific FaultException if services throw them
                {
                    Console.WriteLine($"A service fault occurred: {faultEx.Detail.Message}");
                    // Abort the channel if it faulted
                    if (client.State != CommunicationState.Closed)
                    {
                        client.Abort();
                        Console.WriteLine("Client channel aborted due to fault.");
                    }
                }
                catch (CommunicationException commEx)
                {
                    Console.WriteLine($"A communication error occurred: {commEx.Message}");
                    // Abort the channel if it faulted
                    if (client.State != CommunicationState.Closed)
                    {
                        client.Abort();
                        Console.WriteLine("Client channel aborted due to communication error.");
                    }
                }
                catch (Exception ex)
                {
                    // Catch any other unexpected errors
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    // Abort the channel if it faulted
                    if (client.State != CommunicationState.Closed)
                    {
                        client.Abort();
                        Console.WriteLine("Client channel aborted due to unexpected error.");
                    }
                }
            } // The 'using' statement's Dispose() method will call Close() or Abort()

            // --- Migration Challenge Hint: Client Interaction ---
            // This WCF client proxy code is specific to System.ServiceModel.
            // When migrating the client in modern .NET, you might replace this
            // with HttpClient for HTTP services or a gRPC client for RPC services.
            // For CoreWCF NetTcp, you might still use a WCF client, but configured
            // programmatically rather than via App.config XML.
            // The logic for handling service responses and errors will also change.


            Console.WriteLine("\nPress any key to exit the client.");
            Console.ReadKey();
        }
    }
}