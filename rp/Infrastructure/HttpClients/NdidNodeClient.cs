using System.Text;
using System.Text.Json;

namespace ndid_poc.Infrastructure.HttpClients;

public class NdidNodeClient
{
    private readonly IHttpClientFactory _factory;
    private readonly ILogger<NdidNodeClient> _logger;
    private readonly string _ndidNodeUrl;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented        = true
    };

    public NdidNodeClient(IHttpClientFactory factory, ILogger<NdidNodeClient> logger, IConfiguration config)
    {
        _factory     = factory;
        _logger      = logger;
        _ndidNodeUrl = config["Ndid:NodeUrl"] ?? "http://localhost:8080";
    }

    public async Task<(string RequestId, bool IsMock)> CreateRequestAsync(
        string @namespace, string identifier, string referenceId, string callbackUrl)
    {
        var body = new
        {
            reference_id    = referenceId,
            callback_url    = callbackUrl,
            min_ial         = "2.1",
            min_aal         = "1",
            min_idp         = 1,
            request_timeout = 3600,
            request_message = "กรุณายืนยันตัวตนเพื่อทดสอบระบบ"
        };

        var content = new StringContent(JsonSerializer.Serialize(body, JsonOpts), Encoding.UTF8, "application/json");
        var http    = _factory.CreateClient();
        http.BaseAddress = new Uri(_ndidNodeUrl);

        _logger.LogInformation("Sending request: namespace={Namespace} id={Identifier}", @namespace, identifier);

        try
        {
            var res          = await http.PostAsync($"/rp/requests/{@namespace}/{identifier}", content);
            var responseBody = await res.Content.ReadAsStringAsync();

            _logger.LogInformation("NDID response [{StatusCode}]: {Body}", (int)res.StatusCode, responseBody);

            if (!res.IsSuccessStatusCode)
                throw new HttpRequestException($"NDID node error: {responseBody}", null, res.StatusCode);

            var requestId = JsonSerializer.Deserialize<JsonElement>(responseBody)
                                         .GetProperty("request_id")
                                         .GetString()!;
            return (requestId, false);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogWarning("Cannot reach NDID node ({Message}) — returning mock response", ex.Message);
            return ($"mock-{Guid.NewGuid()}", true);
        }
    }
}
