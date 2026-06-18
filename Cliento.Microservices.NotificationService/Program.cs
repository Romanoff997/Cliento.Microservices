using Cliento.Microservices.CRMService.Cache;
using Cliento.Microservices.CRMService.Data;
using Cliento.Microservices.NotificationService.Services;
using Cliento.Microservices.Shared.Messaging;
using Cliento.Microservices.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CrmDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetValue<string>("ConnectionStrings:Postgres")));

var rabbitMqSettings = new RabbitMqSettings();
builder.Configuration.GetSection("RabbitMq").Bind(rabbitMqSettings);
// RabbitMQ publisher (host from config)
builder.Services.AddSingleton<IEventPublisher>(sp =>
    new RabbitMqPublisher(rabbitMqSettings));

builder.Services.AddSingleton<INativeConsoleNotificationSender>(sp =>
    new ConsoleNotificationSender());

// Add Cors etc.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();
