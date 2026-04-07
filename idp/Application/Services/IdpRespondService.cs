using System.Text;
using System.Text.Json;
using ndid_idp.Infrastructure.Repositories;

namespace ndid_idp.Application.Services;

public class IdpRespondService(IdpRequestRepository repository, IHttpClientFactory factory, IConfiguration config, ILogger<IdpRespondService> logger)
{
    private readonly string _ndidNodeUrl = config["Ndid:NodeUrl"] ?? "http://localhost:8080";

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented        = true
    };

    public async Task<string> RespondAsync(string requestId, string action)
    {
        var record = repository.Get(requestId);
        if (record is null) return "not_found";

        var body = new
        {
            reference_id = Guid.NewGuid().ToString(),
            callback_url = config["Ndid:CallbackUrl"] ?? "http://localhost:5101/ndid/callback",
            namespace_   = "citizen_id",
            identifier   = "mock",
            ial          = "2.1",
            aal          = "1",
            status       = action == "accept" ? "accept" : "reject",
            signature    = "mock-signature",
            accessor_id  = "mock-accessor-id"
        };

        var content = new StringContent(JsonSerializer.Serialize(body, JsonOpts), Encoding.UTF8, "application/json");
        var http    = factory.CreateClient();
        http.BaseAddress = new Uri(_ndidNodeUrl);

        try
        {
            var res = await http.PostAsync($"/idp/response", content);
            var status = res.IsSuccessStatusCode ? action : "error";
            repository.Save(requestId, body, status);
            logger.LogInformation("IdP responded: requestId={RequestId} action={Action}", requestId, action);
            return status;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning("Cannot reach NDID node ({Message}) — mock respond", ex.Message);
            repository.Save(requestId, body, action);
            return $"mock-{action}";
        }
    }
}