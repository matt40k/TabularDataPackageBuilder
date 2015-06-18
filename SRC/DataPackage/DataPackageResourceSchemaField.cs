using System;
using System.Data;
using System.Runtime.Serialization;

[Serializable]
public class DataPackageResourceSchemaField
{
    [DataMember]
    public string Name { get; set; }

    /// <summary>
    /// The type of the field (string, number etc) - see below for more detail. If type is not provided a consumer should assume a type of "string".
    /// </summary>
    [DataMember]
    public string Type { get; set; }

    /// <summary>
    /// A nicer human readable label or title for the field
    /// </summary>
    [DataMember]
    public string Title { get; set; }

    /// <summary>
    /// A description for this field e.g. "The recipient of the funds"
    /// </summary>
    [DataMember]
    public string Description { get; set; }

    /// <summary>
    /// A description of the format e.g. “DD.MM.YYYY” for a date. See below for more detail.
    /// </summary>
    [DataMember]
    public string Format { get; set; }

    /// <summary>
    /// A constraints descriptor that can be used by consumers to validate field values
    /// </summary>
    [DataMember]
    public DataPackageResourceSchemaFieldConstraints Constraints { get; set; }

    public int Length
    {
        get
        {
            if (DbType == DbType.String)
            {
                return 4000;
            }

            return 0;
        }
    }

    public int Precision
    {
        get
        {
            if (DbType == DbType.Decimal)
            {
                return 18;
            }

            return -1;
        }
    }

    public int Scale
    {
        get
        {
            if (DbType == DbType.Decimal)
            {
                return 0;
            }

            return -1;
        }
    }

    public DbType DbType
    {
        get
        {
            if (string.IsNullOrEmpty(Type)) { return DbType.String; }

            var cleanType = Type.Trim().ToLowerInvariant();
            switch (cleanType)
            {
                case "boolean": return DbType.Boolean;
                case "integer": return DbType.Int32;
                case "number": return DbType.Decimal;
                case "string": return DbType.String;
                case "datetime": return DbType.DateTime2;
                case "date": return DbType.Date;
                case "time": return DbType.Time;
                default: throw new NotSupportedException("The Type '" + Type + "' is not supported presently.");
            }
        }
        set
        {
            switch (value)
            {
                case DbType.String:
                    Type = "string";
                    break;
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                    Type = "integer";
                    break;
                case DbType.Currency:
                case DbType.Decimal:
                    Type = "number";
                    break;
                case DbType.Boolean:
                    Type = "boolean";
                    break;
                case DbType.Date:
                    Type = "date";
                    break;
                case DbType.Time:
                    Type = "time";
                    break;
                case DbType.DateTime:
                case DbType.DateTime2:
                    Type = "datetime";
                    break;
                default:
                    Type = "string";
                    break;
                //throw new NotSupportedException("We Don't Support that type yet");
            }
        }
    }
}