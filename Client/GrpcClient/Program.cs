using API.Protos;
using Grpc.Net.Client;
//call
using var grpcChannel = GrpcChannel.ForAddress("http://localhost:5251/");


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

var InsertToPersonsResponse = client.InsertToPersons(_PersonInsertRequest);


Console.WriteLine(InsertToPersonsResponse.Message);

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
Console.WriteLine(_PersonUpdateResponse.Message);

//delete
RemoveRequest _RemoveRequest = new()
{
   
    Id = 4

};
var _PersonRemoveResponse = client.RemovePersons(_RemoveRequest);
Console.WriteLine(_PersonRemoveResponse.Message);

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