using RestSharp;
using VirusTotalAPI.Exceptions;
using VirusTotalAPI.Models;

namespace VirusTotalAPI;

public abstract class Endpoint
{
    protected RestClient _client;
    protected string _url = "https://www.virustotal.com/api/v3";
    protected string ApiKey
    {
        get => ApiKey;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Api key shouldn't be empty.");
            }

            ApiKey = value;
        }
    }

    // https://docs.virustotal.com/reference/errors
    protected void ThrowErrorResponseException(ErrorResponse error)
    {
        throw error.Code switch
        {
            "AuthenticationRequiredError" => new AuthenticationRequiredException(error.Message),
            "BadRequestError" => new BadRequestException(error.Message),
            "InvalidArgumentError" => new InvalidArgumentException(error.Message),
            "NotAvailableYet" => new NotAvailableYetException(error.Message),
            "UnselectiveContentQueryError" => new UnselectiveContentQueryException(error.Message),
            "UnsupportedContentQueryError" => new UnsupportedContentQueryException(error.Message),
            "UserNotActiveError" => new UserNotActiveException(error.Message),
            "WrongCredentialsError" => new WrongCredentialsException(error.Message),
            "ForbiddenError" => new ForbiddenException(error.Message),
            "AlreadyExistsError" => new AlreadyExistsException(error.Message),
            "FailedDependencyError" => new FailedDependencyException(error.Message),
            "QuotaExceededError" => new QuotaExceededException(error.Message),
            "TooManyRequestsError" => new TooManyRequestsException(error.Message),
            "TransientError" => new TransientException(error.Message),
            "DeadlineExceededError" => new DeadlineExceededException(error.Message),
            _ => new Exception(error.Message)
        };
    }
}
