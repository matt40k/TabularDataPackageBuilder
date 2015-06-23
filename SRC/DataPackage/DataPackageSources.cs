using System;
using System.Runtime.Serialization;

[Serializable]
public class DataPackageSources
{
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Url { get; set; }

    [DataMember]
    public string Email { get; set; }
}