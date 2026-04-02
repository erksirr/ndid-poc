namespace ndid_poc.API.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => new
        {
            message = "NDID RP Starter — running!",
            endpoints = new[]
            {
                "POST /verify                          → ส่ง request ไปหา NDID node",
                "POST /ndid/callback                   → รับ callback จาก NDID node",
                "GET  /result/{id}                     → ดูผลลัพธ์",
                "POST /mock/callback/{id}?status=...   → inject mock callback"
            }
        });
    }
}
