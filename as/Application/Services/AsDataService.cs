using System.Text;
using System.Text.Json;
using ndid_as.Infrastructure.Repositories;

namespace ndid_as.Application.Services;

public class AsDataService(AsRequestRepository repository, IHttpClientFactory factory, IConfiguration config, ILogger<AsDataService> logger)
{
    private readonly string _ndidNodeUrl = config["Ndid:NodeUrl"] ?? "http://localhost:8080";

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        WriteIndented        = true
    };

    public async Task<string> SendDataAsync(string requestId)
    {
        var record = repository.Get(requestId);
        if (record is null) return "not_found";

        // Mock data — ในระบบจริงดึงจาก database ของ AS
        var data = new
        {
            reference_id = Guid.NewGuid().ToString(),
            callback_url = config["Ndid:CallbackUrl"] ?? "http://localhost:5102/ndid/callback",
            data = new[]
            {
                new { name = "ชื่อ-สกุล", value = "สมชาย ใจดี" },
                new { name = "วันเกิด",   value = "1990-01-01" }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(data, JsonOpts), Encoding.UTF8, "application/json");
        var http    = factory.CreateClient();
        http.BaseAddress = new Uri(_ndidNodeUrl);

        try
        {
            var res = await http.PostAsync($"/as/data/{requestId}", content);
            var status = res.IsSuccessStatusCode ? "sent" : "error";
            repository.Save(requestId, data, status);
            logger.LogInformation("AS sent data: requestId={RequestId}", requestId);
            return status;
        }
        catch (HttpRequestException ex)
        {
            logger.LogWarning("Cannot reach NDID node ({Message}) — mock send", ex.Message);
            repository.Save(requestId, data, "mock-sent");
            return "mock-sent";
        }
    }
}