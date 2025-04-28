using System.ServiceModel;

namespace TodoServiceHost
{    
    /* DISCLAIMER: This is not being used, only kept as reference */

    // [ServiceContract]
    // This attribute marks the IServiceContractExample interface as a WCF service contract.
    // A service contract defines the set of operations (methods) that the service exposes to clients.
    // It's like an interface for a web service, specifying what the service can do.
    [ServiceContract]
    public interface IServiceContractExample
    {
        // [OperationContract]
        // This attribute marks the DoWork method as a service operation.
        // Each method in a service contract that you want to expose as a callable web service method
        // must be decorated with the [OperationContract] attribute.
        [OperationContract]
        void DoWork();
    }
}