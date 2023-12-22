using System;
using System.Runtime.Serialization;

namespace SharedContracts.Model;
[DataContract]
public class AuthResponse
{
    [DataMember(Order = 1)]
    public string Token { set; get; }

    [DataMember(Order = 2)]
    public Int32 Expires { set; get; }

}
