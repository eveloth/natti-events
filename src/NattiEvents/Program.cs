using Chemodanchik.ApiConfiguration;
using Microsoft.EntityFrameworkCore;
using NattiEvents.Data;
using NattiEvents.Installers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.InstallSerilog();

builder.Services.AddHttpContextAccessor();

builder.InstallAuthentication();
builder.Services.AddAuthorization();
builder.InstallSubdomainWildcardCorsPolicy();

builder.Services.AddDbContext<NattiEventsContext>(
    optionsBuilder => optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

app.ConfigureForwardedHeaders();
app.UseSerilogRequestLogging();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.RunAsync();
