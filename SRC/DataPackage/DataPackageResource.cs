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
    public string Hash { get; set; }

    [DataMember]
    public long Bytes { get; set; }

    [DataMember]
    public DataPackageResourceSchema Schema { get; set; }
}