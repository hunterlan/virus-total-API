﻿using System.Net.Http.Json;
using System.Text.Json;
using RestSharp;
using VirusTotalCore.Exceptions;
using VirusTotalCore.Models;

namespace VirusTotalCore;

public abstract class BaseEndpoint
{
    protected readonly HttpClient HttpClient;
    protected readonly RestClient Client;
    private readonly string _url = "https://www.virustotal.com/api/v3/";
    private readonly string _apiKey = null!;

    public BaseEndpoint(string apiKey, string endpoint)
    {
        _url += endpoint;
        ApiKey = apiKey;
        HttpClient = new HttpClient
        {
            BaseAddress = new Uri(_url),
        };
        HttpClient.DefaultRequestHeaders.Add("x-apikey", ApiKey);
        var options = new RestClientOptions(_url);
        Client = new RestClient(options);
        Client.AddDefaultHeader("x-apikey", ApiKey);
    }
    
    protected string ApiKey
    {
        get => _apiKey;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Api key shouldn't be empty.");
            }

            _apiKey = value;
        }
    }

    protected readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented = true
    };

    // https://docs.virustotal.com/reference/errors
    protected static Exception ThrowErrorResponseException(ErrorResponse error)
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
        var errorResponse = errorJsonDocument.RootElement.GetProperty("error").Deserialize<ErrorResponse>(JsonSerializerOptions)!;
        return ThrowErrorResponseException(errorResponse);
    }

    protected Task<RestResponse> GetResponse(RestRequest request, CancellationToken? cancellationToken)
    {
        return cancellationToken is not null
            ? Client.ExecuteGetAsync(request, cancellationToken.Value)
            : Client.ExecuteGetAsync(request);
    }

    protected Task<RestResponse> PostResponse(RestRequest request, CancellationToken? cancellationToken)
    {
        return cancellationToken is not null
            ? Client.ExecutePostAsync(request, cancellationToken.Value)
            : Client.ExecutePostAsync(request);
    }

    protected async Task<T> GetAsync<T>(string requestUrl, CancellationToken cancellationToken)
    {
        using var response = await HttpClient.GetAsync(requestUrl, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.Deserialize<T>(JsonSerializerOptions)!;
        return result;
    }

    protected async Task<T> GetAsync<T>(string requestUrl, string jsonRootName, CancellationToken cancellationToken)
    {
        using var response = await HttpClient.GetAsync(requestUrl, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
        
        var resultJsonDocument = JsonDocument.Parse(resultJson);
        var result = resultJsonDocument.RootElement.GetProperty(jsonRootName).Deserialize<T>(JsonSerializerOptions)!;
        return result;
    }

    protected async Task DeleteAsync(string requestUrl, CancellationToken cancellationToken)
    {
        using var response = await HttpClient.DeleteAsync(requestUrl, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
    }

    protected async Task PostAsync(string requestUrl, object value, CancellationToken cancellationToken)
    {
        var response = await HttpClient.PostAsJsonAsync(requestUrl, value, cancellationToken);
        var resultJson = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response is not { IsSuccessStatusCode: true })
        {
            throw HandleError(resultJson);
        }
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
        var parameters = new { limit, cursor };
        var requestUrl = $"{classObjectApiValue}/{relationship}";
        
        var request = new RestRequest(requestUrl).AddObject(parameters);
        
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        return restResponse.Content!;
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
        var parameters = new { limit, cursor };
        var requestUrl = $"{classObjectApiValue}/relationships/{relationship}";
        
        var request = new RestRequest(requestUrl).AddObject(parameters);
        
        var restResponse = await GetResponse(request, cancellationToken);

        if (restResponse is { IsSuccessful: false }) throw HandleError(restResponse.Content!);

        return restResponse.Content!;
    }
}
