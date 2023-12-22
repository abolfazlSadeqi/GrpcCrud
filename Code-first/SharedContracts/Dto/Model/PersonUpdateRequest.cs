using System;
using System.Runtime.Serialization;

namespace SharedContracts.Model;
[DataContract]
public class PersonUpdateRequest
{
    [DataMember(Order = 1)]
    public int Id { get; set; }

    [DataMember(Order = 2)]
    public string FirstName { get; set; }

    [DataMember(Order = 3)]
    public string LastName { get; set; }

    [DataMember(Order = 4)]
    public string Suffix { get; set; }

    [DataMember(Order = 5)]
    public string Email { get; set; }

    [DataMember(Order = 6)]
    public string AdditionalContactInfo { get; set; }
}
