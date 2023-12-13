namespace VirusTotalAPI.Exceptions;

/// <summary>
/// Exception for NotFoundError response: "The requested resource was not found."
/// </summary>
/// <param name="message">Error message</param>
public class NotFoundException(string message) : Exception(message);