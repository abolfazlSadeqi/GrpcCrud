using System.Runtime.Serialization;

namespace SharedContracts.Model;

[DataContract]
public class ChangeResponse
{
    [DataMember(Order = 1)]
    public string message { get; set; }

    [DataMember(Order = 2)]
    public statusresultenum statusresult { get; set; }

}
