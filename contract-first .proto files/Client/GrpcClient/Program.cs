using API.Protos;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using GrpcClient;
//call


//Use Transient fault handling with gRPC retries

var defaultMethodConfig = new MethodConfig
{
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy
    {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(1),
        MaxBackoff = TimeSpan.FromSeconds(5),
        BackoffMultiplier = 1.5,
        RetryableStatusCodes = { StatusCode.Unavailable }
    }
};

//Keep alive pings
var handler = new SocketsHttpHandler
{
    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
    EnableMultipleHttp2Connections = true
};



/*
 without Interceptor

using var grpcChannel = GrpcChannel.ForAddress("http://localhost:5251/", new GrpcChannelOptions
{
    HttpHandler = handler,
    ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } }
});
var client = new PersonService.PersonServiceClient(grpcChannel);

with Intercept

using var grpcChannel = GrpcChannel.ForAddress("http://localhost:5251/");

var invoker = grpcChannel.Intercept(new TestInterceptor());
var client = new PersonService.PersonServiceClient(invoker);
 */



using var grpcChannel = GrpcChannel.ForAddress("http://localhost:5251/");
var AuthClient = new AuthService.AuthServiceClient(grpcChannel);
var AuthResponse = AuthClient.CheckAuth(new AuthRequest
{
    UserName = "test1",
    Password = "test@Test1"
});


var headers = new Metadata();
headers.Add("Authorization", $"Bearer {AuthResponse.Token}");
//var sumResult = calculationClient.Add(new InputNumbers { Number1 = 5, Number2 = 10 }, headers);
//Console.WriteLine($"Sum Result: 5+10={sumResult.Result}");

//var subtractResult = calculationClient.Subtract(new InputNumbers { Number1 = 20, Number2 = 5 }, headers);
//Console.WriteLine($"Subtract result: 20-5={subtractResult.Result}");

var client = new PersonService.PersonServiceClient(grpcChannel);



//insert
PersonInsertRequest _PersonInsertRequest = new()
{
    AdditionalContactInfo = "testrrr2",
    Email = "aaa@sss.com",
    FirstName = "test542",
    LastName = "Familytest2",
    Suffix = "Develop2",

};

var InsertToPersonsResponse = client.InsertToPersons(_PersonInsertRequest, headers);


Console.WriteLine(InsertToPersonsResponse.Statusresult);

//Update
PersonUpdateRequest _PersonUpdateRequest = new()
{
    AdditionalContactInfo = "testrrr",
    Email = "aaa@sss.com",
    FirstName = "test54",
    LastName = "Familytest",
    Suffix = "tester",
    Id = 3

};

var _PersonUpdateResponse = client.UpdatePerson(_PersonUpdateRequest);
Console.WriteLine(_PersonUpdateResponse.Statusresult);

//delete
RemoveRequest _RemoveRequest = new()
{
   
    Id = 4

};
var _PersonRemoveResponse = client.RemovePersons(_RemoveRequest);
Console.WriteLine(_PersonRemoveResponse.Statusresult);

//GetByid
PersonGetByIdRequest _PersonGetByIdRequest = new()
{
    Id = 1
};

var PersonFirst = client.PersonGetById(_PersonGetByIdRequest);

Console.WriteLine(PersonFirst.FirstName);


//Getall
Empty _Empty = new()
{

};

var _Person = client.PersonGetall(_Empty);

Console.WriteLine(_Person.PersonResponses.Count);

Console.ReadKey();