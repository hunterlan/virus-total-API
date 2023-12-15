namespace VirusTotalCore.Models.Analysis;

public class EngineAnalysisResult
{
    /// <summary>
    /// Normalized result.
    /// </summary>
    public required string Category { get; set; }

    /// <summary>
    /// Type of service given by that URL scanning service
    /// </summary>
    public required string Result { get; set; }

    /// <summary>
    /// raw value returned by the URL scanner ("clean", "malicious", "suspicious", "phishing").
    /// It may vary from scanner to scanner, hence the need for the "category" field for normalisation.
    /// </summary>
    public required string Method { get; set; }
}