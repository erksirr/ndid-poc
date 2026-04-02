using ndid_poc.Infrastructure.Repositories;

namespace ndid_poc.API.Endpoints;

public static class ResultEndpoints
{
    public static void MapResultEndpoints(this WebApplication app)
    {
        app.MapGet("/result/{requestId}", (string requestId, PostgresNdidRequestRepository repository) =>
        {
            if (!repository.TryGet(requestId, out var result))
                return Results.NotFound(new { message = "Request ID not found" });

            return Results.Ok(result);
        });
    }
}
