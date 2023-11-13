using API.Protos;
using AutoMapper;
using Azure.Core;
using DAL.Model;
using DAL.Repository;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System;

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

    public override Task<ItemPersonResponse> PersonGetall(Empty request, ServerCallContext context)
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
                CreateDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(item.CreateDate),
                ModifiedDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(item.ModifiedDate)

            });
        }

        return Task.FromResult(ItemPersonResponses);
    }



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

            return Task.FromResult(new ChangeResponse() { Message = "success", Statuscode = "0" });
        }
        catch
        {
            return Task.FromResult(new ChangeResponse() { Message = "error", Statuscode = "-1" });
        }
    }

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


            return Task.FromResult(new ChangeResponse() { Message = "success", Statuscode = "0" });
        }
        catch
        {
            return Task.FromResult(new ChangeResponse() { Message = "error", Statuscode = "-1" });
        }
    }


    public override Task<ChangeResponse> RemovePersons(Protos.RemoveRequest request, ServerCallContext context)
    {
        try
        {

            unitOfWork.Person.Remove(unitOfWork.Person.GetById(request.Id));
            unitOfWork.Save();


            return Task.FromResult(new ChangeResponse() { Message = "success", Statuscode = "0" });
        }
        catch
        {
            return Task.FromResult(new ChangeResponse() { Message = "error", Statuscode = "-1" });
        }
    }
}