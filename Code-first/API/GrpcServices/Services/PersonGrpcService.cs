
using DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using ProtoBuf.Grpc;
using SharedContracts.Interface;
using SharedContracts.Model;

namespace API.GrpcServices.Services;

public class PersonGrpcService : PersonServiceBase
{
    private readonly IUnitOfWork unitOfWork;
    public PersonGrpcService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
   
    }

    [Authorize(Roles = "PersondataRole")]
    public  Task<PersonResponse> PersonGetById(PersonGetByIdRequest request, CallContext context = default)
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
            personGetByIdResponse.CreateDate = d.CreateDate;
            personGetByIdResponse.ModifiedDate = d.ModifiedDate;


        }

        return Task.FromResult(personGetByIdResponse);
    }
    [Authorize(Roles = "PersondataRole")]
    public  Task<ItemPersonResponse> PersonGetall(Empty request, CallContext context = default)
    {

        ItemPersonResponse ItemPersonResponses = new ItemPersonResponse();
        var _list = unitOfWork.Person.GetAll();
        foreach (var item in _list)
        {
            ItemPersonResponses.PersonResponses = new PersonResponse()
            {
                AdditionalContactInfo = item.AdditionalContactInfo,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Suffix = item.Suffix,
                CreateDate = item.CreateDate,
                ModifiedDate = item.ModifiedDate

            };
        }

        return Task.FromResult(ItemPersonResponses);
    }


    [Authorize(Roles = "PersondataRole")]
    public  Task<ChangeResponse> InsertToPersons(PersonInsertRequest request, CallContext context = default)
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

            return Task.FromResult(new ChangeResponse() { message = "", statusresult = statusresultenum.Sucess });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ChangeResponse() { message = ex.Message, statusresult = statusresultenum.Fail });
        }
    }

    [Authorize(Roles = "test")]
    public  Task<ChangeResponse> UpdatePerson(PersonUpdateRequest request, CallContext context = default)
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


            return Task.FromResult(new ChangeResponse() { message = "", statusresult = statusresultenum.Sucess });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ChangeResponse() { message = ex.Message, statusresult = statusresultenum.Fail });
        }
    }

    [Authorize(Roles = "PersondataRole")]
    public  Task<ChangeResponse> RemovePersons(RemoveRequest request, CallContext context = default)
    {
        try
        {

            unitOfWork.Person.Remove(unitOfWork.Person.GetById(request.Id));
            unitOfWork.Save();


            return Task.FromResult(new ChangeResponse() { message = "", statusresult = statusresultenum.Sucess });
        }
        catch (Exception ex)
        {
            return Task.FromResult(new ChangeResponse() { message = ex.Message, statusresult = statusresultenum.Fail });
        }
    }
}