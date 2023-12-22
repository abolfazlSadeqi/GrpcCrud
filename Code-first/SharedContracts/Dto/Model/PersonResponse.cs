using System.Runtime.Serialization;

namespace SharedContracts.Model;
[DataContract]
public class PersonResponse
{
    [DataMember(Order = 1)]
    public string FirstName { get; set; }

    [DataMember(Order = 2)]
    public string LastName { get; set; }

    [DataMember(Order = 3)]
    public string Suffix { get; set; }

    [DataMember(Order = 4)]

    public string Email { get; set; }

    [DataMember(Order = 5)]
    public string AdditionalContactInfo { get; set; }

    [DataMember(Order = 6)]
    public DateTime CreateDate { get; set; }

    [DataMember(Order = 7)]
    public DateTime ModifiedDate { get; set; }
}
