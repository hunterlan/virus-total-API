namespace VirusTotalCore.Exceptions;
/// <summary>
/// Exception for AuthenticationRequiredException response: "ApiKey is missing"
/// </summary>
/// <param name="message">Error message</param>
public class AuthenticationRequiredException(string message) : Exception(message);