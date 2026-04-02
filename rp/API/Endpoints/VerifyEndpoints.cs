using ndid_poc.Application.DTOs;
using ndid_poc.Application.Services;

namespace ndid_poc.API.Endpoints;

public static class VerifyEndpoints
{
    public static void MapVerifyEndpoints(this WebApplication app)
    {
        app.MapPost("/verify", async (VerifyInput input, VerifyService service) =>
        {
            var response = await service.VerifyAsync(input);
            return Results.Ok(response);
        });
    }
}
