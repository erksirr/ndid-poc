namespace ndid_as.API.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => new
        {
            message = "NDID AS — running!",
            endpoints = new[]
            {
                "POST /ndid/callback          → รับ request จาก NDID node",
                "GET  /requests               → ดู pending requests",
                "POST /requests/{id}/send     → ส่งข้อมูลกลับ NDID node",
                "GET  /requests/{id}          → ดูสถานะ request"
            }
        });
    }
}