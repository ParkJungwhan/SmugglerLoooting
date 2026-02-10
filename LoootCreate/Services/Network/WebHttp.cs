namespace LoootCreate.Services.Network;

public class WebHttp
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public async Task<string> GetHttpAsync(
        string url,
        Dictionary<string, string>? headers = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        using var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public string GetHttp(
        string url,
        Dictionary<string, string>? headers = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        using var response = _httpClient
            .SendAsync(request)
            .GetAwaiter()
            .GetResult();

        response.EnsureSuccessStatusCode();

        return response.Content
            .ReadAsStringAsync()
            .GetAwaiter()
            .GetResult();
    }
}