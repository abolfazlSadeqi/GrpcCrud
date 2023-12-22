using SharedContracts.Model;


using ProtoBuf.Grpc;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SharedContracts.Interface;

[ServiceContract]
public interface PersonServiceBase
{
    [OperationContract]
    public  Task<PersonResponse> PersonGetById(PersonGetByIdRequest request, CallContext context = default);

    [OperationContract]
    public  Task<ItemPersonResponse> PersonGetall(Empty request, CallContext context = default);

    [OperationContract]
    public  Task<ChangeResponse> InsertToPersons(PersonInsertRequest request, CallContext context = default);

    [OperationContract]
    public  Task<ChangeResponse> UpdatePerson(PersonUpdateRequest request, CallContext context = default);

    [OperationContract]
    public  Task<ChangeResponse> RemovePersons(RemoveRequest request, CallContext context = default);

}

