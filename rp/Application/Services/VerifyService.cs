using ndid_poc.Application.DTOs;
using ndid_poc.Infrastructure.HttpClients;
using ndid_poc.Infrastructure.Repositories;

namespace ndid_poc.Application.Services;

public class VerifyService(NdidNodeClient ndidNodeClient, PostgresNdidRequestRepository repository, IConfiguration config)
{
    private readonly string _callbackUrl = config["Ndid:CallbackUrl"] ?? "http://localhost:5100/ndid/callback";

    public async Task<VerifyResponse> VerifyAsync(VerifyInput input)
    {
        var refId = Guid.NewGuid().ToString();
        var (requestId, isMock) = await ndidNodeClient.CreateRequestAsync(
            input.Namespace, input.Identifier, refId, _callbackUrl);

        repository.Save(requestId, new
        {
            status       = "pending",
            request_id   = requestId,
            reference_id = refId
        });

        return new VerifyResponse(
            RequestId:   requestId,
            ReferenceId: refId,
            Note:        isMock ? "MOCK — NDID node ไม่ได้รัน ใช้สำหรับทดสอบ flow เท่านั้น" : null,
            NextStep:    $"GET /result/{requestId}");
    }
}
