using System.Data;

public class CsvList
{
    public bool Selected { get; set; }
    public string Filename { get; set; }
    public bool InPackage { get; set; }
}

public class CsvColumn
{
    public string Name { get; set; }
    public string Type { get; set;  }
}