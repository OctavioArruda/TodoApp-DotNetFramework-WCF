// Use this code for TodoServiceHost/TodoServiceSvcFactory.cs

using System;
using System.ServiceModel; // Required for ServiceHost
using System.ServiceModel.Activation; // Required for ServiceHostFactory
using TodoServiceLibrary; // Required to reference your service implementation and dependencies

// Make sure this namespace matches your TodoServiceHost project's namespace
namespace TodoServiceHost
{
    // --- Custom ServiceHostFactory for TodoService.svc ---
    // Inherit from ServiceHostFactory because our .svc file's Service attribute
    // specifies a service TYPE name ("TodoServiceLibrary.TodoService").
    public class TodoServiceSvcFactory : ServiceHostFactory
    {
        // Override the CreateServiceHost method with the signature that receives the Service TYPE.
        // This method is called by the WCF hosting environment when a client
        // accesses TodoService.svc, because the .svc file's Service attribute
        // points to TodoServiceLibrary.TodoService Type.
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            // We expect serviceType to be typeof(TodoService) here.
            // Optional check:
            // if (serviceType != typeof(TodoService)) { /* Handle unexpected type */ }


            // --- Dependency Injection Setup (Manual Composition Root for TodoService.svc) ---
            // Create the service instance and its dependencies here.

            Console.WriteLine($"[{DateTime.Now}] TodoServiceSvcFactory: Creating service instance for {serviceType.FullName} (for TodoService.svc)...");

            // Manually create the dependency required by TodoService.
            var demoDependency = new DeprecatedDependencyInjectionDemo();

            // Manually create the service instance, injecting the dependency.
            var serviceInstance = new TodoService(demoDependency);

            Console.WriteLine($"[{DateTime.Now}] TodoServiceSvcFactory: Service instance created.");
            // --- End Manual Composition Root ---


            // Create and return the ServiceHost instance, passing the pre-created instance.
            // Remember the ServiceHost(instance, baseAddresses) constructor requires
            // the service type (TodoService) to have InstanceContextMode.Single,
            // which we already added with the [ServiceBehavior] attribute.
            return new ServiceHost(serviceInstance, baseAddresses);
        }
    }
}