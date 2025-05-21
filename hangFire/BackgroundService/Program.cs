using BackgroundService.Auth;
using BackgroundService.RabbitMqPublisher;
using Hangfire;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Adiciona o Hangfire
builder.Services.AddHangfire(config => config
    .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddHangfireServer();

var app = builder.Build();

// Redireciona a raiz para a URL /hangfire
app.MapGet("/", () => Results.Redirect("/hangfire"));

// Configura o Hangfire Dashboard com autentica��o b�sica
//app.UseHangfireDashboard("/hangfire", new DashboardOptions
//{
//    Authorization = new[] { new BasicAuthAuthorizationFilter() }  // Aplica a autentica��o
//});

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate("job-teste", () => Publisher.SendMessage("Executando job!"), Cron.Minutely);

app.Run();
