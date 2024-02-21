namespace VirusTotalCore.Models.Analysis;

public class AnalysisReport<T>
{
    public required string Type { get; set; }
    public required string Id { get; set; }
    
    public required T Attributes { get; set; }
}