using System;
using System.Runtime.Serialization;

namespace SharedContracts.Model;
[DataContract]
public class PersonGetByIdRequest
{
    [DataMember(Order = 1)]
    public int Id { get; set; }
}

