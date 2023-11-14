Create a simple CRUD application with Groc  :


## Definition 
 is a modern open source high performance Remote Procedure Call (RPC) framework that can run in any environment
### benefits
•	Modern, high-performance, lightweight RPC framework.

•	Contract-first API development, using Protocol Buffers by default, allowing for language agnostic implementations.

•	Tooling available for many languages to generate strongly-typed servers and clients.

•	Supports client, server, and bi-directional streaming calls.

•	Reduced network usage with Protobuf binary serialization.

 
### Protocol Buffers 
Protocol Buffers(protobuf)  are Google’s language-neutral, platform-neutral extensible mechanisms for serializing structured data.
gRPC uses a contract-first approach to API development. Using this file, we can complete this approach

The .proto file contains:
|Name|Definition|		
|-|-|	
|	message 	|	Specify the data to be exchanged ,It can also be used nested	|
|enum	|list of values|
|One of	|If you have a message with many fields and where at most one field will be set at the same time, you can enforce this behavior and save memory by using the oneof feature.	|
|map|a paired key/value field type. like dictionary.	map<key, value> mapName = number;|
|Packages|	You can add an optional package specifier to a .proto file to prevent name clashes between protocol message types. 	|
|Services|	You can specify the structure of the methods|

•	If you do not have an output or input, you must create an empty Message ,message Empty {}

•	You can add Other file with import

•	Map fields cannot be repeated


### Scalar Value Types

|.proto Type|	C# Type|	.proto| 	C# |
|-|	-|	-| 	- |
|double|	double|	float	|float|
|int32	|int	|int64	|long|
|uint32|	uint|	uint64|	ulong|
|sint32|	int|	sint64|	long|
|fixed32|	uint|	fixed64|	ulong|
|sfixed32|	int|	sfixed64|	long|
|bool	|bool	|string|	string|
|Datetime 1.Add import "google/protobuf/timestamp.proto"; 2.use in messagegoogle.protobuf.Timestamp   Name = N;	|in C# FromDateTimeOffset(FieldName)	|||

### Testing (You can test with the following tool

    1.Postman 2.gRPCurl  3.gRPCui 

### gRPC calls

•	Unary RPCs: in which the client sends only one request to the server and receives a single response.

•	Server streaming RPCs: The client sends the request to the server and receives a stream to read a sequence of messages.

•	Client Streaming RPCs: The client writes a sequence of messages and sends them to the server, again using a provided stream.

•	Bidirectional Streaming RPCs: Both parties send a sequence of messages using a read-write stream.


## Steps implement

### Server
1.Install-Package Grpc.AspNetCore 

2.Add Person.proto File 
```
syntax = "proto3";

import "google/protobuf/timestamp.proto";
option csharp_namespace = "API.Protos";

//for GetByID
message PersonGetByIdRequest {
	int32 Id = 1;
}

//enum test
enum statusresultenum {
  Sucess = 0;
  Fail = 1;
}
//message To response Create and Remove And Update
message ChangeResponse {
	string message = 1;
	statusresultenum statusresult = 2;
}
message RemoveRequest {
	int32 Id = 1;
}

//Response GetByid and GetAll Person
message PersonResponse {
	optional string FirstName = 1;
    optional	string LastName = 2;
    optional	string Suffix = 3;
    optional	string Email = 4;
	optional string AdditionalContactInfo = 5;
	google.protobuf.Timestamp   CreateDate = 6;
	google.protobuf.Timestamp   ModifiedDate = 7;	
}

message ItemPersonResponse {
    repeated PersonResponse PersonResponses = 1;
}

//request To insert and Update
message PersonInsertRequest {
	 string FirstName = 1;
    	string LastName = 2;
    	string Suffix = 3;
    	string Email = 4;
	 string AdditionalContactInfo = 5;
}

message PersonUpdateRequest {
	 int32 Id=1;
	 string FirstName = 2;
     string LastName = 3;
     string Suffix = 4;
     string Email = 5;
	 string AdditionalContactInfo = 6;
}
message Empty {}
//List Method
service PersonService {
	rpc PersonGetById(PersonGetByIdRequest) returns (PersonResponse);
	rpc  PersonGetall( Empty ) returns (ItemPersonResponse);

	rpc  InsertToPersons(PersonInsertRequest) returns (ChangeResponse);

	rpc  UpdatePerson(PersonUpdateRequest) returns (ChangeResponse);

	rpc  RemovePersons(RemoveRequest) returns (ChangeResponse);

}
```
3.Build Project To Create Files related C#

4.Add Service file with inherit ([ServiceName in Proto File].[ ServiceName in Proto File Base]) and override All rpc in Service File proto

```
namespace API.GrpcServices;

public class PersonGrpcService : PersonService.PersonServiceBase
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper _mapper;
    public PersonGrpcService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        _mapper = mapper;
    }
}
```

#### GetByID(Get Person by PersonId)

```
    public override Task<PersonResponse> PersonGetById(PersonGetByIdRequest request, ServerCallContext context)
    {


        PersonResponse personGetByIdResponse = new PersonResponse();
        var d = unitOfWork.Person.GetById(request.Id);
        if (d is not null)
        {
            personGetByIdResponse.AdditionalContactInfo = d.AdditionalContactInfo;
            personGetByIdResponse.FirstName = d.FirstName;
            personGetByIdResponse.LastName = d.LastName;
            personGetByIdResponse.Email = d.Email;
            personGetByIdResponse.Suffix = d.Suffix;
            personGetByIdResponse.CreateDate = Timestamp.FromDateTime(d.CreateDate);
            personGetByIdResponse.ModifiedDate = Timestamp.FromDateTime(d.ModifiedDate);


        }
       
        return Task.FromResult(personGetByIdResponse);
    }
```

#### GetAllPerson(Getall Person)
```
public override Task<ItemPersonResponse> PersonGetall(Protos.Empty request, ServerCallContext context)
    {
        ItemPersonResponse ItemPersonResponses = new ItemPersonResponse();
        var _list = unitOfWork.Person.GetAll();
        foreach (var item in _list)
        {
            ItemPersonResponses.PersonResponses.Add(new PersonResponse()
            {
                AdditionalContactInfo = item.AdditionalContactInfo,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Suffix = item.Suffix,
                CreateDate = Timestamp.FromDateTimeOffset(item.CreateDate),
                ModifiedDate = Timestamp.FromDateTimeOffset(item.ModifiedDate)

            });
        }

        return Task.FromResult(ItemPersonResponses);
    }
```

#### InsertToPerson 

```
    [Authorize(Roles = "Administrator")]
    public override Task<ChangeResponse> InsertToPersons(Protos.PersonInsertRequest request, ServerCallContext context)
    {
        try
        {
            ItemPersonResponse ItemPersonResponses = new ItemPersonResponse();
            unitOfWork.Person.Add(new DAL.Model.Person()
            {
                AdditionalContactInfo = request.AdditionalContactInfo,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Suffix = request.Suffix,
                CreateDate = DateTime.Now,
                ModifiedDate = DateTime.Now,

            });
            unitOfWork.Save();

            return Task.FromResult(new ChangeResponse() { Message = "", Statusresult = statusresultenum.Sucess });
        }
        catch(Exception ex)
        {
            return Task.FromResult(new ChangeResponse() { Message = ex.Message, Statusresult = statusresultenum.Fail });
        }
    }
```

##### UpdatePerson

```
    [Authorize(Roles = "test")]
    public override Task<ChangeResponse> UpdatePerson(Protos.PersonUpdateRequest request, ServerCallContext context)
    {
        try
        {

            var Personold = unitOfWork.Person.GetById(request.Id);
            Personold.LastName = request.LastName;
            Personold.FirstName = request.FirstName;
            Personold.Suffix = request.Suffix;
            Personold.Email = request.Email;
            Personold.AdditionalContactInfo = request.AdditionalContactInfo;
            Personold.ModifiedDate = DateTime.Now;


            unitOfWork.Save();


            return Task.FromResult(new ChangeResponse() { Message = "", Statusresult = statusresultenum.Sucess });
        }
        catch(Exception ex)
        {
            return Task.FromResult(new ChangeResponse() { Message = ex.Message, Statusresult = statusresultenum.Fail });
        }
    }
```

#### RemovePersons

```
    public override Task<ChangeResponse> RemovePersons(Protos.RemoveRequest request, ServerCallContext context)
    {
        try
        {

            unitOfWork.Person.Remove(unitOfWork.Person.GetById(request.Id));
            unitOfWork.Save();


            return Task.FromResult(new ChangeResponse() { Message = "", Statusresult = statusresultenum.Sucess });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ChangeResponse() { Message = ex.Message, Statusresult = statusresultenum.Fail });
        }
    }}
```

 5.Change compiler File
right Click To file >Change Build Actio To Protobuf compiler>Change Grpc stub Classes To Server Only

6.Add Configre 
```
services.AddGrpc();
app.UseEndpoints(endpoints =>
{
   //ServiceName
    endpoints.MapGrpcService<PersonGrpcService>();
});
```

### Client

1. Install packages on nuget

     a.Grpc.Tools b.Grpc.Net.Client c.Google.Protobuf
   
2.Add proto files in “Add Connected Service.”

3.Build Projects

4.Create Channel and Call file Service

```
using var grpcChannel = GrpcChannel.ForAddress(http://localhost:5251/ );
var client = new PersonService.PersonServiceClient(grpcChannel);
```

5.Call Method

##### //insert
```
PersonInsertRequest _PersonInsertRequest = new(){
    AdditionalContactInfo = "testrrr2",Email = "aaa@sss.com",
    FirstName = "test542",LastName = "Familytest2",Suffix = "Develop2",
};

var InsertToPersonsResponse = client.InsertToPersons(_PersonInsertRequest);

Console.WriteLine(InsertToPersonsResponse.Statusresult);
```

#### //Update

```
PersonUpdateRequest _PersonUpdateRequest = new()
{
    AdditionalContactInfo = "testrrr",Email = "aaa@sss.com",
    FirstName = "test54",LastName = "Familytest", = "tester",Id = 3
};

var _PersonUpdateResponse = client.UpdatePerson(_PersonUpdateRequest);
Console.WriteLine(_PersonUpdateResponse.Statusresult);

```
#### //Remove

```
RemoveRequest _RemoveRequest = new(){Id = 4 };
var _PersonRemoveResponse = client.RemovePersons(_RemoveRequest);
Console.WriteLine(_PersonRemoveResponse.Statusresult);
```

#### //GetByid

```
PersonGetByIdRequest _PersonGetByIdRequest = new(){Id = 1};
var PersonFirst = client.PersonGetById(_PersonGetByIdRequest);
Console.WriteLine(PersonFirst.FirstName);

```

#### //Getall
```

Empty _Empty = new(){};
var _Person = client.PersonGetall(_Empty);
Console.WriteLine(_Person.PersonResponses.Count);

```
## Interceptors

gRPC concept that allows apps to interact with incoming or outgoing gRPC calls. They offer a way to enrich the request processing pipeline.Use to log and ErrorHandler and etc

 Type 1.servers 2.clients

### Client (Interceptor methods to override for client)

1.BlockingUnaryCall:	Intercepts a blocking invocation of an unary RPC.

2.AsyncUnaryCall:Intercepts an asynchronous invocation of an unary RPC.

3.AsyncClientStreamingCall:Intercepts an asynchronous invocation of a client-streaming RPC

4.AsyncServerStreamingCall:	Intercepts an asynchronous invocation of a server-streaming RPC

5.AsyncDuplexStreamingCall	:Intercepts an asynchronous invocation of a bidirectional-streaming RPC

#### Steps implement Interceptor  in Client

1.add class  Interceptor and override Method

```

public class TestInterceptor : Interceptor
{

    public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        Console.WriteLine($"MethodName= {context.Method.Name} , Date={DateTime.Now.ToString()}");
        return base.BlockingUnaryCall(request, context, continuation);
    }
    }

```

2.add Config

```

using var grpcChannel = GrpcChannel.ForAddress("http://localhost:5251/");
var invoker = grpcChannel.Intercept(new TestInterceptor());
var client = new PersonService.PersonServiceClient(invoker);
```

### Server (methods to override for server)

1.UnaryServerHandler:	Intercepts a unary RPC

2.ClientStreamingServerHandler:	Intercepts a client-streaming RPC

3.ServerStreamingServerHandler:	Intercepts a server-streaming RPC

4.DuplexStreamingServerHandler:	Intercepts a bidirectional-streaming RPC

Steps implement Interceptor  in Server

1.Add class Interceptor

```
public class ServerTestInterceptor : Interceptor
{
   
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
       
        Console.WriteLine($"MethodName= {context.Method} , Date={DateTime.Now.ToString()}");
        return await continuation(request, context);
    }
}

```

2.Configure server interceptors

```

services.AddGrpc(options =>
 {
     options.Interceptors.Add<ServerTestInterceptor>(); });

```

### Use Transient fault handling with gRPC retries

1. Definition

```

var defaultMethodConfig = new MethodConfig{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
    MaxAttempts = 5, InitialBackoff = TimeSpan.FromSeconds(1), MaxBackoff = TimeSpan.FromSeconds(5),
        BackoffMultiplier = 1.5, RetryableStatusCodes = { StatusCode.Unavailable }
    }
};

```

2. use in Client

```

using var grpcChannel = GrpcChannel.ForAddress("http://localhost:5251/", new GrpcChannelOptions
{
        ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } }
});

```

## Authentication and authorization (Use jwt Jwt and Identity)

### Steps implement 
#### Server

1.Add Config Jwt and Identity

  include
•	add Identity(add Tables and config Contexts in EF core)

•	add Authentication in Startup include (add AddAuthentication, AddJwtBearer,…) 

•	add jwt config in appsettings

2.add New Proto File(Auth) and Method(Check User and Password) To Login 

```

syntax = "proto3";

option csharp_namespace = "API.Protos";

package authentication;

message AuthRequest{
	string UserName = 1;
	string Password = 2;
}

message AuthResponse{
	string Token = 1;
	int32 Expires = 2;
}

service AuthService {
	rpc CheckAuth(AuthRequest) returns (AuthResponse);

}
```


3.add Service File(class) to Check user with use jwt and Identity

```

public class AuthGrpcService : AuthService.AuthServiceBase
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<ApplicationRole> _roleManager;
    public AuthGrpcService(UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager; _configuration = configuration;
        this._roleManager = roleManager;
    }

 public override async Task<AuthResponse> CheckAuth(AuthRequest request, ServerCallContext context)
     => await Login(request);
}
```


4.add Authorize attribute in Method  

```

[Authorize(Roles = "")]
```

### Client

1.Add New Proto File(Auth)

2.call CheckAuth(login) in AuthService and Get Token

```

using var grpcChannel = GrpcChannel.ForAddress("http://localhost:5251/");
var AuthClient = new AuthService.AuthServiceClient(grpcChannel);
var AuthResponse = AuthClient.CheckAuth(new AuthRequest
{
    UserName = "test1",
    Password = "test@Test1"
});
```


3.send Token To Method(Create Header and Send Header To Method)

```

var headers = new Metadata();
headers.Add("Authorization", $"Bearer {AuthResponse.Token}");
```


#### //insert

```

PersonInsertRequest _PersonInsertRequest = new()
{
    AdditionalContactInfo = "testrrr2",
    Email = "aaa@sss.com",
    FirstName = "test542",
    LastName = "Familytest2",
    Suffix = "Develop2",

};

var InsertToPersonsResponse = client.InsertToPersons(_PersonInsertRequest, headers);
```




