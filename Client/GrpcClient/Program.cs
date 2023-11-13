using API.Protos;
using Grpc.Net.Client;

using var grpcChannel = GrpcChannel.ForAddress("http://localhost:5251/");


var client = new PersonService.PersonServiceClient(grpcChannel);

PersonGetByIdRequest _PersonGetByIdRequest = new()
{
    Id = 1
};

var PersonFirst = client.PersonGetById(_PersonGetByIdRequest);

Console.WriteLine(PersonFirst.FirstName);



Empty _Empty = new()
{
    
};

var _Person = client.PersonGetall(_Empty);

Console.WriteLine(_Person.PersonResponses.Count);

Console.ReadKey();