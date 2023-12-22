using System.Runtime.Serialization;

namespace SharedContracts.Model;
[DataContract]
public class ItemPersonResponse
{
    [DataMember(Order = 1)]
    public PersonResponse PersonResponses { get; set; }
}
