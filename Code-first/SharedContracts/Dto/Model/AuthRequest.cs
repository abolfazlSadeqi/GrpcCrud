using ProtoBuf.Grpc;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SharedContracts.Model;

[DataContract]
public class AuthRequest
{
    [DataMember(Order = 1)]

    public string UserName { set; get; }


    [DataMember(Order = 2)]
    public string Password { set; get; }
}
