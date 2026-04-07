using System.Text.Json;
using ndid_idp.Application.Services;

namespace ndid_idp.API.Endpoints;

public static class CallbackEndpoints
{
    public static void MapCallbackEndpoints(this WebApplication app)
    {
        app.MapPost("/ndid/callback", (JsonElement payload, IdpCallbackService service) =>
        {
            service.HandleIncomingRequest(payload);
            return Results.Ok();
        });
    }
}