using Microsoft.EntityFrameworkCore;
using ndid_as.API.Endpoints;
using ndid_as.Application.Services;
using ndid_as.Infrastructure.Persistence;
using ndid_as.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<AsRequestRepository>();
builder.Services.AddScoped<AsCallbackService>();
builder.Services.AddScoped<AsDataService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

app.MapHealthEndpoints();
app.MapCallbackEndpoints();
app.MapRequestEndpoints();

app.Run("http://localhost:5102");