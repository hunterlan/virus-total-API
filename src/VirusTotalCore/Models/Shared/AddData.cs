namespace VirusTotalCore.Models.Shared;

public class AddData<T>
{
    public required string Type { get; set; }
    
    public required T Attributes { get; set; }
}