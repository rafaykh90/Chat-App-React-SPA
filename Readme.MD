# Chat Application React using SignalR

This is a .Net Core Single Page Chat Application using React as the Client App which uses SignalR. The backend serves a Websocket end-point using SignalR and some basic Web API end-points to fetch initial data. Online users are tracked using a Redis Distributed Cache and the Chat messages are persisted in Azure Table Storage.

### Controllers
- ##### UsersController

    - CheckIfUserExist 
    Used to Check if the user already exists with the name at the time of Registration.
    * **URL** : `/api/Users/exists?name`
    * **Method** : `GET`
    * **Auth required** : NO
    

    - LoggedOnUsers
    Fetches the List of Online users from Redis Cache using UserTracker.
    * **URL** : `/api/Users/LoggedOnUsers`
    * **Method** : `GET`
    * **Auth required** : NO
    
    
- ##### ChatController

    - InitialMessages
    Fetches Top 100 messages from the Storage Table.
    * **URL** : `/api/Chat/InitialMessages`
    * **Method** : `GET`
    * **Auth required** : NO


### SignalR Hub
`ChatHub.cs` is derived from the abstract class `Hub.cs` present in `Microsoft.AspNetCore.SignalR`. In this class methods can be implementede which are invoked from the Client App.
    
##### Methods
 - `AddMessage(string username, string message)` - This method adds the message to the Storage Table and then invokes the method 'MessageAdded' to all the Clients to update the UI.
 - `UserConnected(string username)` - This methods adds the user to the Online user list in the cache and invokes the 'UserLoggedOn' method to all the Clients.

    Other Methods invoked from the backend to the Clients include `UserLoggedOn` and `UserLoggedOff` which are invoked on new user connected and when users leaves respectively.

### ChatService and ChatRepository
`ChatService` uses and instance of `ChatRepository` which is injected in the class as a dependency. The purpose of the `ChatRepository` class is just to Add or Get messages from the Azure Table Storage, whereas, `ChatService` itself acts as a wrapper on the repository. This way if we are to use a new Storage type e.g. SQL, we can just create a new repository and inject it in the `ChatService`

`ChatMessageTableEntity.cs` is a model class derived from `TableEntity`, which is the base class to store entites in the Azure Table Storage.

`ChatService` and `ChatRepository` implemts interfaces `IChatService` and `IChatRepository` which makes it easier to used with Dependency Injection.

### UserTracker
`UserTracker` is similar to `ChatService` and is used to Add users to the Online List in Redis and Get the online users list from the cache. This class implements `IUserTracker`

The project uses the DI pattern and injects dependencies in constructors, which is provided by `Microsoft.Extensions.DependencyInjection`, and is also used to achieve IoC between classes and their dependencies.