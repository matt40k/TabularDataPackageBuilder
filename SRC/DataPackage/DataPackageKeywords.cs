using System;
using System.Runtime.Serialization;

[Serializable]
public class DataPackageKeywords
{
    [DataMember]
    public string Keyword { get; set; }
}