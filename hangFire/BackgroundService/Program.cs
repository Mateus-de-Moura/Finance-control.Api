using BackgroundService.Infra;
using BackgroundService.Services;
using BackgroundService.Services.Mail;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<FinanceControlContex>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfire(config => config
    .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"))
);


builder.Services.AddScoped<ExpensesService>();
builder.Services.AddScoped<MailSender>();
builder.Services.AddHangfireServer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<FinanceControlContex>();
    database.Database.Migrate();
}

app.MapGet("/", () => Results.Redirect("/hangfire"));

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    // O dashboard e exposto pelo Compose somente em 127.0.0.1.
    Authorization = Array.Empty<IDashboardAuthorizationFilter>()
});

RecurringJob.AddOrUpdate<ExpensesService>(
    "job-notify",
    service => service.CheckExpensesAndNotify(),
    "0 */3 * * *"
);

RecurringJob.AddOrUpdate<ExpensesService>(
    "job-expenses",
    service => service.RegisterExpensesIsRecurrent(),
    "0 0 30 * *"
);

RecurringJob.AddOrUpdate<ExpensesService>(
    "job-delete-old-locationData",
    service => service.DeleteOldLocationData(),
    "0 */3 * * *"
);

app.Run();
