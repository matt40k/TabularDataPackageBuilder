using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class DataPackageResourceSchema
{
    [DataMember]
    public ICollection<DataPackageResourceSchemaField> Fields { get; set; }
}