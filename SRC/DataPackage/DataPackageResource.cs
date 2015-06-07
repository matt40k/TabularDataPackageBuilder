using System;
using System.Runtime.Serialization;

[Serializable]
public class DataPackageResource
{
    [DataMember]
    public string Url { get; set; }

    [DataMember]
    public string Path { get; set; }

    [DataMember]
    public DataPackageResourceSchema Schema { get; set; }
}