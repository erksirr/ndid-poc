namespace ndid_idp.API.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => new
        {
            message = "NDID IdP — running!",
            endpoints = new[]
            {
                "POST /ndid/callback              → รับ request จาก NDID node",
                "GET  /requests                   → ดู pending requests",
                "POST /requests/{id}/respond      → ตอบ accept/reject",
                "GET  /requests/{id}              → ดูสถานะ request"
            }
        });
    }
}