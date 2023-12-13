namespace VirusTotalAPI.Exceptions;

public class TooManyRequestsException(string message) : Exception(message);