namespace VirusTotalCore.Exceptions;

public class TooManyRequestsException(string message) : Exception(message);