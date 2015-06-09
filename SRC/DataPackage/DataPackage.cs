using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class DataPackage
{
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public ICollection<DataPackageResource> Resources { get; set; }

    [DataMember]
    public ICollection<DataPackageKeywords> Keywords { get; set; }

    [DataMember]
    public ICollection<DataPackageSources> Sources { get; set; }

    [DataMember]
    public string License { get; set; }

    [DataMember]
    public string Version { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string Description { get; set; }

    [DataMember]
    public string LastUpdated { get; set; }
}