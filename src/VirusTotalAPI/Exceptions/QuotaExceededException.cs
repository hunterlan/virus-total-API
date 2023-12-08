namespace VirusTotalAPI.Exceptions;

public class QuotaExceededException(string message) : Exception(message);