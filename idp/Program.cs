using Microsoft.EntityFrameworkCore;
using ndid_idp.API.Endpoints;
using ndid_idp.Application.Services;
using ndid_idp.Infrastructure.Persistence;
using ndid_idp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IdpRequestRepository>();
builder.Services.AddScoped<IdpCallbackService>();
builder.Services.AddScoped<IdpRespondService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

app.MapHealthEndpoints();
app.MapCallbackEndpoints();
app.MapRequestEndpoints();

app.Run("http://localhost:5101");