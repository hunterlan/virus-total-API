using System.Net.Http.Json;
using System.Text.Json;
using VirusTotalCore.Exceptions;
using VirusTotalCore.Models;

namespace VirusTotalCore;

public abstract class BaseEndpoint
{
    protected readonly HttpClient HttpClient;
    protected readonly string CurrentEndpointName;
    private readonly string _apiKey = null!;
    
    private const string Url = "https://www.virustotal.com/api/v3/";

    protected BaseEndpoint(string apiKey, string endpoint)
    {
        CurrentEndpointName = endpoint;
        ApiKey = apiKey;
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(Url),
        };
        HttpClient.DefaultRequestHeaders.Add("x-apikey", ApiKey);
    }

    protected BaseEndpoint(HttpClient customHttpClient, string apiKey, string endpoint)
    {
        CurrentEndpointName = endpoint;
        ApiKey = apiKey;
        HttpClient = customHttpClient;
        HttpClient.BaseAddress = new Uri(Url);
        HttpClient.DefaultRequestHeaders.Add("x-apikey", ApiKey);
    }

    protected string ApiKey
    {
        get => _apiKey;
        private init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Api key shouldn't be empty.");
            }

            _apiKey = value;
        }
    }

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true
    };

    // https://docs.virustotal.com/reference/errors
    private static Exception ThrowErrorResponseException(ErrorResponse error)
    {
        return error.Code switch
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
            "NotFoundError" => new NotFoundException(error.Message),
            _ => new Exception(error.Message)
        };
    }

    protected Exception HandleError(string errorContent)
    {
        var errorJsonDocument = JsonDocument.Parse(errorContent);
        var errorResponse = errorJsonDocument.RootElement.GetProperty("error").Deserialize<ErrorResponse>(_jsonSerializerOptions)!;
        return ThrowErrorResponseException(errorResponse);
    }

    protected async Task<T> GetAsync<T>(string requestUrl, CancellationToken cancellationToken)
    {
        var finalUrl = BuildRelativeUrl(requestUrl);
        using var response = await HttpClient.GetAsync(finalUrl, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.Deserialize<T>(_jsonSerializerOptions)!;
        return result;
    }

    protected async Task<T> GetAsync<T>(string requestUrl, string jsonRootName, CancellationToken cancellationToken)
    {
        var finalUrl = BuildRelativeUrl(requestUrl);
        using var response = await HttpClient.GetAsync(finalUrl, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.RootElement.GetProperty(jsonRootName).Deserialize<T>(_jsonSerializerOptions)!;
        return result;
    }

    protected async Task DeleteAsync(string requestUrl, CancellationToken cancellationToken)
    {
        var finalUrl = BuildRelativeUrl(requestUrl);
        using var response = await HttpClient.DeleteAsync(finalUrl, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
    }

    protected async Task PostAsync(string requestUrl, object value, CancellationToken cancellationToken)
    {        
        var finalUrl = BuildRelativeUrl(requestUrl);
        var response = await HttpClient.PostAsJsonAsync(finalUrl, value, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
    }

    protected async Task<T> PostAsync<T>(string requestUrl, object value, CancellationToken cancellationToken)
    {
        var finalUrl = BuildRelativeUrl(requestUrl);
        var response = await HttpClient.PostAsJsonAsync(finalUrl, value, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.Deserialize<T>(_jsonSerializerOptions)!;
        return result;
    }
    
    protected async Task<T> PostAsync<T>(string requestUrl, string jsonRootName, object value, CancellationToken cancellationToken)
    {
        var finalUrl = BuildRelativeUrl(requestUrl);
        var response = await HttpClient.PostAsJsonAsync(finalUrl, value, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.RootElement.GetProperty(jsonRootName).Deserialize<T>(_jsonSerializerOptions)!;
        return result;
    }

    /// <summary>
    /// This endpoint allows to get objects relationship to another objects. 
    /// </summary>
    /// <param name="classObjectApiValue">Value of specific endpoint. For example, for IP it's IP value</param>
    /// <param name="relationship">Relationship name. See VirusTotal relationship table for specific endpoint.</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="limit">Maximum number of related objects to retrieve</param>
    /// <returns>JSON string</returns>
    /// <exception cref="Exception"></exception>
    public virtual async Task<string> GetRelatedObjects(string classObjectApiValue, string relationship, string? cursor, 
        CancellationToken? cancellationToken, int limit = 10)
    {
        var requestUrl = $"{CurrentEndpointName}/{classObjectApiValue}/comments?limit={limit}";
        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }
        
        using var response = await HttpClient.GetAsync(requestUrl, cancellationToken ?? new CancellationToken());
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken ?? new CancellationToken());
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }

        return resultJson;
    }

    /// <summary>
    /// Get related object's IDs
    /// </summary>
    /// <param name="classObjectApiValue">Value of specific endpoint. For example, for IP it's IP value</param>
    /// <param name="relationship">Relationship name. See VirusTotal relationship table for specific endpoint.</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="limit">Maximum number of related objects to retrieve</param>
    /// <returns>JSON string</returns>
    /// <exception cref="Exception"></exception>
    public async Task<string> GetRelatedDescriptors(string classObjectApiValue, string relationship, string? cursor, 
        CancellationToken? cancellationToken, int limit = 10)
    {
        var requestUrl = $"{CurrentEndpointName}/{classObjectApiValue}/relationships/{relationship}?limit={limit}";
        if (cursor is not null)
        {
            requestUrl += $"&cursor={cursor}";
        }
        
        using var response = await HttpClient.GetAsync(requestUrl, cancellationToken ?? new CancellationToken());
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken ?? new CancellationToken());
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }

        return resultJson;
    }

    private string BuildRelativeUrl(string requestUrl)
    {
        return requestUrl.StartsWith('?') ? $"{CurrentEndpointName}{requestUrl}" : $"{CurrentEndpointName}/{requestUrl}";
    }
}
