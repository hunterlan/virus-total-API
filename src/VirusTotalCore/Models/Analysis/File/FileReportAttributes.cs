namespace VirusTotalCore.Models.Analysis.File;

public class FileReportAttributes
{
    public required string TypeDescription { get; set; }
    /// <summary>
    /// tlsh is a hash used by Trend Micro which can be used for similarity comparisons.
    /// </summary>
    public required string Tlsh { get; set; }
    /// <summary>
    /// In-house similarity clustering algorithm value, based on a simple structural feature hash allows you to find similar files.
    /// </summary>
    public required string Vhash { get; set; }
    public required IEnumerable<string> TypeTags { get; set; }
    public required IEnumerable<string> Names { get; set; }
    public long LastModificationDate { get; set; }
    public required string TypeTag { get; set; }
    public long TimesSubmitted { get; set; }
    public required Votes TotalVotes { get; set; }
    public long Size { get; set; }
    public required string TypeExtension { get; set; }
    public long LastSubmissionDate { get; set; }
    public required Dictionary<string, EngineAnalysisResult> LastAnalysisResults { get; set; }
    public required string Sha256 { get; set; }
    public required IEnumerable<string> Tags { get; set; }
    public long LastAnalysisDate { get; set; }
    public long UniqueSources { get; set; }
    public long FirstSubmissionDate { get; set; }
    /// <summary>
    /// ssdeep is a program for computing Context Triggered Piecewise Hashes. Also called fuzzy hashes,
    /// it allows identifying similar files by comparing (via Edit Distance) their hashes.
    /// </summary>
    public required string Ssdeep { get; set; }
    public required string Md5 { get; set; }
    public required string Sha1 { get; set; }
    /// <summary>
    /// magic gives a guess of the file type, based on a popular parsing tool from unix.
    /// </summary>
    public required string Magic { get; set; }
    public required ScanAnalysisStats LastAnalysisStats { get; set; }
    public required string MeaningfulName { get; set; }
    public long Reputation { get; set; }
}