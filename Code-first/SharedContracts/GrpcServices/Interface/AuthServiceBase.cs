using ProtoBuf.Grpc;
using SharedContracts.Model;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;


namespace SharedContracts.Interface;
[ServiceContract]
public interface AuthServiceBase
{
    [OperationContract]
    public  Task<AuthResponse> CheckAuth(AuthRequest request, CallContext context = default);

}

