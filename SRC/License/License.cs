using System;
using System.Runtime.Serialization;

[Serializable]
public class License
{
    [DataMember]
    public bool Domain_Content { get; set; }

    [DataMember]
    public bool Domain_Data { get; set; }

    [DataMember]
    public bool Domain_Software { get; set; }

    [DataMember]
    public string Family { get; set; }

    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Maintainer { get; set; }

    [DataMember]
    public string OD_Conformance { get; set; }

    [DataMember]
    public string OSD_Conformance { get; set; }

    [DataMember]
    public string Status { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string Url { get; set; }
}
