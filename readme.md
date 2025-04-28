# 📝 Todo App (Legacy .NET Framework 4.7 with WCF)

This project demonstrates a simple Todo application built using .NET Framework 4.7 and Windows Communication Foundation (WCF). It's designed for educational purposes, showcasing how WCF was traditionally used for service-oriented architecture in the .NET ecosystem.

**⚠️ Important Note:** WCF is considered a legacy technology. While many applications still use it, Microsoft recommends using ASP.NET Core gRPC or other modern alternatives for new development. This project is for learning and maintenance contexts only.

## 📂 Project Structure

The solution contains two projects:

* `TodoServiceLibrary`: This is the WCF Service Library project, containing the service contract (`ITodoService`) and its implementation (`TodoService`).
* `TodoServiceTestClient`: A console application used for basic, in-solution testing of the service.

## ✨ Key Technologies

* .NET Framework 4.7: The target framework for the application.
* WCF (Windows Communication Foundation): Microsoft's framework for building service-oriented applications.

## ⚙️ WCF Explained

WCF is a powerful framework that enables communication between distributed applications. It unifies various communication technologies (like web services, remoting, messaging) into a single programming model.

### Core Concepts

* **Service:** A component that exposes operations (methods) that other applications can call. In our case, `TodoService` is the service.
* **Contract:** Defines the operations a service offers. The `ITodoService` interface is the contract.
    * `[ServiceContract]`: Attribute that marks an interface as a service contract.
    * `[OperationContract]`: Attribute that marks a method within a service contract as a service operation (a method that can be called by clients).
* **Endpoint:** Specifies *how* a client can communicate with a service. Each endpoint has an address, binding, and contract.
    * `Address`: Where the service is located (e.g., a URL).
    * `Binding`: How the service communicates (protocol, encoding).
        * `basicHttpBinding`: For basic web service communication (HTTP, text-based). Good for interoperability.
        * `wsHttpBinding`: For more advanced web service features (security, transactions).
        * `netTcpBinding`: For .NET-to-.NET communication over TCP (faster).
        * `netNamedPipeBinding`: For communication on the same machine (very fast).
    * `Contract`: The interface that defines the service operations.
* **Message:** The unit of communication (data sent between client and service).
* **Hosting:** The process of making a service available (e.g., in IIS, a Windows service, a console application).

### Code Walkthrough

#### `ITodoService.cs` (Service Contract)

```csharp
[ServiceContract]
public interface ITodoService
{
    [OperationContract]
    TodoItem GetTodoItem(int id);
    // ... other operations
}
```

* The `[ServiceContract]` attribute defines this interface as a WCF service contract.
* `[OperationContract]` attributes mark each method as a callable service operation.
* `TodoItem`: A Data Transfer Object (DTO) to represent a Todo item.
    * `[DataContract]` and `[DataMember]` attributes are used to control how data is serialized (converted to a format for transmission).

#### `TodoService.cs` (Service Implementation)

```csharp
public class TodoService : ITodoService
{
    // ... implementation of ITodoService methods
}
```

* This class *implements* the `ITodoService` interface, providing the actual logic for each operation (e.g., getting data, adding data, etc.).

#### `App.config` (Service Configuration)

```xml
<system.serviceModel>
    <services>
        <service name="TodoServiceLibrary.TodoService" behaviorConfiguration="TodoServiceBehavior">
            <endpoint address="" binding="basicHttpBinding" contract="TodoServiceLibrary.ITodoService" />
            <endpoint address="mex" binding="mexHttpBinding" contract="IMetadataExchange" />
            <host>
                <baseAddresses>
                    <add baseAddress="http://localhost:8733/TodoServiceLibrary/TodoService" />
                </baseAddresses>
            </host>
        </service>
    </services>
    <behaviors>
        <serviceBehaviors>
            <behavior name="TodoServiceBehavior">
                <serviceMetadata httpGetEnabled="true" />
                <serviceDebug includeExceptionDetailInFaults="true" />
            </behavior>
        </serviceBehaviors>
    </behaviors>
</system.serviceModel>
```

* **`<system.serviceModel>`:** The root element for WCF configuration.
* **`<services>`:** Defines the services hosted by the application.
    * **`<service>`:** Configures a specific service.
        * `name`: The fully qualified name of the service class.
        * `behaviorConfiguration`: Links to a service behavior.
        * **`<endpoint>`:** Defines how clients access the service.
            * `address`: The endpoint's address (relative to `baseAddress`). `""` means the base address.
            * `binding`: The communication mechanism (e.g., `basicHttpBinding`).
            * `contract`: The service contract interface.
            * `address="mex" ... contract="IMetadataExchange"`: A special endpoint for metadata exchange (client discovery).
        * **`<host>`:** Defines the service's hosting environment.
            * `<baseAddresses>`: The base address(es) for the service.
* **`<behaviors>`:** Defines behaviors that customize service or endpoint behavior.
    * **`<serviceBehaviors>`:** Behaviors for the entire service.
        * `behavior name="TodoServiceBehavior"`: A named behavior.
            * `serviceMetadata httpGetEnabled="true"`: Enables metadata retrieval via HTTP GET.
            * `serviceDebug includeExceptionDetailInFaults="true"`: Controls exception detail sent to clients (for debugging - set to `false` in production!).

## 🏃‍♀️ Running the Application (Testing within the Solution)

The `TodoServiceTestClient` project allows you to test the service logic directly.

1.  **Ensure the `TodoServiceTestClient` is the Startup Project:** Right-click on it in Solution Explorer and select "Set as StartUp Project."
2.  **Build the Solution:** Go to the "Build" menu at the top of Visual Studio. Select "Build Solution" (or press Ctrl + Shift + B).
3.  **Run the Application:** Press Ctrl + F5 (or click the "Start" button, the green play icon).
4.  You'll see console output showing the service methods being called and their results.

**⚠️ Important:** This test client directly instantiates the service class. A real-world client would use a *proxy* generated from the service's metadata.

**Troubleshooting `AddressAccessDeniedException`**

If you encounter the following error when running the `TodoServiceTestClient`:

```
System.ServiceModel.AddressAccessDeniedException: HTTP could not register URL http://+:8733/TodoServiceLibrary/TodoService/. Your process does not have access rights to this namespace...
```

This error indicates that your user account lacks the necessary permissions to listen on the specified HTTP address. This is a security feature in Windows. To resolve it, follow these steps:

1.  **Open an administrator command prompt:**
    * Right-click on the Start button.
    * Select "Windows PowerShell (Administrator)" or "Command Prompt (Administrator)." It's crucial to run it as an administrator.

2.  **Find your Windows username:**
    * There are several ways to find your Windows username. Here are a couple of common methods:
        * **Using the `echo %USERNAME%` command:**
            * In the administrator command prompt, type the following command and press Enter:
                ```bash
                echo %USERNAME%
                ```
            * This command retrieves the value of the `%USERNAME%` environment variable, which stores your current username.
        * **Checking User Account Settings:**
            * You can also find your username in the Windows settings. The exact steps might vary slightly depending on your Windows version:
                * In Windows 10/11, you can typically go to: Start > Settings > Accounts > Your info. Your username is usually displayed there.

3.  **Run the `netsh` command:**
    * Once you have your Windows username, type the following command in the administrator command prompt and press Enter:
        ```bash
        netsh http add urlacl url=http://+:8733/TodoServiceLibrary/TodoService user=YOUR_USERNAME
        ```
        * Replace `YOUR_USERNAME` with the username you obtained in the previous step. **This is case-sensitive, so type it exactly as it appears.**

    * **Explanation of the `netsh` command:**
        * `netsh http`: This is the network shell command-line utility for configuring various HTTP settings.
        * `add urlacl`: This specific `netsh` command is used to add a URL Access Control List (ACL) entry. URL ACLs control which user accounts or applications are allowed to listen on specific HTTP URLs.
        * `url=http://+:8733/TodoServiceLibrary/TodoService`: This specifies the URL that you are granting permissions to.
            * `http://`: Indicates the HTTP protocol.
            * `+`: This wildcard character means that the permission applies to all hostnames (e.g., localhost, your computer's name, or any IP address that resolves to your machine).
            * `8733`: This is the port number that the WCF service is configured to use (as defined in your `App.config`).
            * `/TodoServiceLibrary/TodoService`: This is the path portion of the URL, specifying the location of your service.
        * `user=YOUR_USERNAME`: This specifies the Windows user account to which you are granting the permission to listen on the URL.

4.  **Restart Visual Studio:**
    * Close and reopen Visual Studio to ensure that the changes take effect.

5.  **Run the test client again:**
    * Try running the `TodoServiceTestClient` project. It should now be able to register the URL and run without the `AddressAccessDeniedException`.

## ⏭️ Next Steps

To make the WCF service accessible to other applications (e.g., a web application), you'll need to host it in a suitable environment like IIS (Internet Information Services).

(Further steps on hosting will be provided later.)