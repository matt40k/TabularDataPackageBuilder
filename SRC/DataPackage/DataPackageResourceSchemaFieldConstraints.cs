using System;
using System.Runtime.Serialization;

public class DataPackageResourceSchemaFieldConstraints
{
    /// <summary>
    /// A boolean value which indicates whether a field must have a value in every row of the table. An empty string is considered to be a missing value.
    /// </summary>
    [DataMember]
    public bool? Required { get; set; }

    /// <summary>
    /// An integer that specifies the minimum number of characters for a value
    /// </summary>
    [DataMember]
    public int? MinLength { get; set; }

    /// <summary>
    /// An integer that specifies the maximum number of characters for a value
    /// </summary>
    [DataMember]
    public int? MaxLength { get; set; }

    /// <summary>
    /// A boolean. If true, then all values for that field MUST be unique within the data file in which it is found. This defines a unique key for a row although a row could potentially have several such keys.
    /// </summary>
    [DataMember]
    public bool? Unique { get; set; }

    /// <summary>
    /// A regular expression that can be used to test field values. If the regular expression matches then the value is valid. Values will be treated as a string of characters. It is recommended that values of this field conform to the standard XML Schema regular expression syntax. See also this reference.
    /// </summary>
    [DataMember]
    public string Pattern { get; set; }

    /// <summary>
    /// specifies a minimum value for a field. This is different to minLength which checks number of characters. A minimum value constraint checks whether a field value is greater than or equal to the specified value. The range checking depends on the type of the field. E.g. an integer field may have a minimum value of 100; a date field might have a minimum date. If a minimum value constraint is specified then the field descriptor MUST contain a type key
    /// </summary>
    [DataMember]
    public string Minimum { get; set; }

    /// <summary>
    /// as above, but specifies a maximum value for a field.
    /// </summary>
    [DataMember]
    public string Maximum { get; set; }
}