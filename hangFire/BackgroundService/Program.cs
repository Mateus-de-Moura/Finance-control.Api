using BackgroundService.Infra;
using BackgroundService.Services;
using BackgroundService.Services.Mail;
using Hangfire;
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

app.MapGet("/", () => Results.Redirect("/hangfire"));

app.UseHangfireDashboard();

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
