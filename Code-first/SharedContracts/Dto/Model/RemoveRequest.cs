using System;
using System.Runtime.Serialization;

namespace SharedContracts.Model;
[DataContract]
public class RemoveRequest
{
    [DataMember(Order = 1)]
    public int Id { get; set; }
}
