using api_clean_architecture.Api;
using finance_control.Services.SignalR;
using finance_control.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.AddServices();
builder.AddDatabase();
builder.AddJwtAuth();
builder.AddMapper();
builder.AddInjection();
builder.AddObservability();

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Origens permitidas vêm de Cors:AllowedOrigins (appsettings ou env Cors__AllowedOrigins__0, __1...).
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
if (allowedOrigins is null || allowedOrigins.Length == 0)
    allowedOrigins = ["http://localhost:8085", "http://localhost:5173"];

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy( policy =>
    {
        policy.WithOrigins(allowedOrigins)
               .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
    });
});

var app = builder.Build();

if (builder.Configuration.GetValue<bool>("APPLY_MIGRATIONS"))
{
    using var scope = app.Services.CreateScope();
    var database = scope.ServiceProvider.GetRequiredService<FinanceControlContex>();
    database.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.MapHub<NotificationHub>("/notifyHub");

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
