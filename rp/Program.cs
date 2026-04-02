using Microsoft.EntityFrameworkCore;
using ndid_poc.API.Endpoints;
using ndid_poc.Application.Services;
using ndid_poc.Infrastructure.HttpClients;
using ndid_poc.Infrastructure.Persistence;
using ndid_poc.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// Infrastructure
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<PostgresNdidRequestRepository>();
builder.Services.AddScoped<NdidNodeClient>();

// Application
builder.Services.AddScoped<VerifyService>();
builder.Services.AddScoped<CallbackService>();

var app = builder.Build();

// Auto-migrate on startup
using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

app.MapHealthEndpoints();
app.MapVerifyEndpoints();
app.MapCallbackEndpoints();
app.MapResultEndpoints();
app.MapMockEndpoints();

app.Run("http://localhost:5100");
