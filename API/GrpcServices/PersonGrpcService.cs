using API.Protos;
using AutoMapper;
using DAL.Model;
using DAL.Repository;
using Grpc.Core;

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
        }
        else
        {
            personGetByIdResponse.AdditionalContactInfo = "test";
            personGetByIdResponse.FirstName = "test";
            personGetByIdResponse.LastName = "test";
            personGetByIdResponse.Email = "test";
            personGetByIdResponse.Suffix = "test";
        }
        return Task.FromResult(personGetByIdResponse);
    }

    public override Task<ItemPersonResponse> PersonGetall(Empty request, ServerCallContext context)
    {
        ItemPersonResponse ItemPersonResponses = new ItemPersonResponse();
        var _list = unitOfWork.Person.GetAll();
        foreach ( var item in _list)
        {
            ItemPersonResponses.PersonResponses.Add(new PersonResponse()
            {
                AdditionalContactInfo = item.AdditionalContactInfo,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Email = item.Email,
                Suffix = item.Suffix

            });
        }
       
        

        return Task.FromResult(ItemPersonResponses);
    }
}