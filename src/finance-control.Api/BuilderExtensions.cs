
using System.Configuration;
using System.Data;
using System.Text;
using finance_control.Api.Interfaces;
using finance_control.Api.Services;
using finance_control.Application.UserCQ.Commands;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Interfaces.Repositories;
using finance_control.Infra.Data;
using finance_control.Infra.Data.Repositories;
using finance_control.Services;
using finance_control.Services.AuthService;
using finance_control.Services.BackGroundService;
using finance_control.Services.RabbitMqConsumer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;

namespace api_clean_architecture.Api
{
    public static class BuilderExtensions
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            string connection = configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Finance API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Informe o token JWT no formato: Bearer {seu token}",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            });

            //Serilog
            var options = new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = false
            };

            var columnOptions = new ColumnOptions();

            columnOptions.Store.Add(StandardColumn.LogEvent);
            columnOptions.Store.Remove(StandardColumn.Properties);
            columnOptions.LogEvent.DataLength = 2048;
            columnOptions.Id.DataType = SqlDbType.BigInt;

            builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration                   
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    //.Enrich.WithMachineName()
                    //.Enrich.WithThreadId()
                    .WriteTo.Console(outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm} [{Level}] {Message}{NewLine}{Exception}")
                    .WriteTo.MSSqlServer(
                        connectionString: connection,
                        sinkOptions: options,
                        columnOptions: columnOptions
                    );
            });


            builder.Services.AddMemoryCache();

            builder.Services.AddHostedService<RabbitMqConsumerBackgroundService>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

        }

        public static void AddJwtAuth(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                };
            })
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ExpireTimeSpan = TimeSpan.FromDays(7);
                });
        }

        public static void AddInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IConvertFormFileToBytes, ConvertFormFileToBytes>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ILoginLocationDataRepository, LoginLocationDataRepository>();
            builder.Services.AddSingleton<Consumer>();
            builder.Services.AddScoped<IRevenuesRepository, RevenuesRepository>();
            builder.Services.AddScoped<ITransactionsRepository, TransactionRepository>();

        }

        public static void AddDatabase(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddDbContext<FinanceControlContex>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        //public static void AddValidators(this WebApplicationBuilder builder)
        //{
        //    builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
        //    builder.Services.AddFluentValidationAutoValidation();
        //}

        public static void AddMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
