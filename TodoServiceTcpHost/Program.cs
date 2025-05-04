using System;
using System.ServiceModel; // Required for ServiceHost and CommunicationState
using System.ServiceModel.Description; // Required for ServiceMetadataBehavior (if configured in code)
using TodoServiceLibrary; // Required to reference your service implementation and dependencies

namespace TodoServiceTcpHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // --- WCF Host Implementation ---

            // Base Address for the NetTcp endpoint
            Uri baseAddress = new Uri("net.tcp://localhost:8080/TodoService");

            // --- Dependency Injection Setup (Manual Composition Root) ---
            // Migration Challenge Hint:
            // Because TodoService now uses constructor injection and lacks a
            // parameterless constructor (due to our DI refactoring),
            // the default ServiceHost cannot create it automatically.
            // In .NET Framework without a DI container, you must manually
            // create the service instance (and its dependencies) and pass
            // that instance to the ServiceHost constructor.

            Console.WriteLine("Manually creating service instance and dependencies...");

            // Manually create the dependency (DeprecatedDependencyInjectionDemo)
            // In a real-world scenario with multiple dependencies, this gets complex quickly.
            // If DeprecatedDependencyInjectionDemo had its own dependencies, you'd create those too.
            var demoDependency = new DeprecatedDependencyInjectionDemo();

            // Manually create the service instance, injecting the dependency via the constructor.
            var serviceInstance = new TodoService(demoDependency);

            Console.WriteLine("Service instance created.");
            // --- End Manual Composition Root ---


            // Create the ServiceHost instance
            // We now pass the manually created 'serviceInstance' instead of the service Type (typeof(TodoService)).
            using (ServiceHost host = new ServiceHost(serviceInstance, baseAddress))
            {
                try
                {
                    // The configuration from App.config is still loaded and applied to this host instance.
                    // We don't need to explicitly add endpoints here because they are defined in App.config.

                    // Open the ServiceHost
                    host.Open();

                    Console.WriteLine($"The Todo Service is ready at {baseAddress}");
                    Console.WriteLine("Press <ENTER> to stop the service.");
                    Console.ReadLine();

                    // Close the ServiceHost when the user presses Enter
                    if (host.State != CommunicationState.Faulted) // Good practice to only close if not faulted
                    {
                        host.Close();
                        Console.WriteLine("The Todo Service is closed.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions during hosting
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    // If the host faulted, abort it
                    if (host.State != CommunicationState.Closed)
                    {
                        host.Abort();
                    }
                }
            }

            // --- Migration Challenge Hint: Hosting with DI ---
            // In CoreWCF (.NET 8), the built-in DI container handles this.
            // You register your Service (TodoService) and its dependencies
            // (DeprecatedDependencyInjectionDemo, or ideally interfaces for them)
            // in the DI container during host startup. The CoreWCF hosting
            // automatically resolves and creates service instances using the container
            // as needed, eliminating this manual creation step in the host code.
        }
    }
}