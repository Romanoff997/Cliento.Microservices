using Cliento.Microservices.CRMService.Cache;
using Cliento.Microservices.CRMService.Data;
using Cliento.Microservices.Shared.Messaging;
using Cliento.Microservices.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerGeneratorOptions.OperationFilters.Add(new AddHeaderParameterOperationFilter());
    // остальные настройки
});

builder.Services.AddDbContext<CrmDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetValue<string>("ConnectionStrings:Postgres")));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("Redis:Configuration")));

builder.Services.AddSingleton<RedisSessionCache>();

var rabbitMqSettings = new RabbitMqSettings();
builder.Configuration.GetSection("RabbitMq").Bind(rabbitMqSettings);
// RabbitMQ publisher (host from config)
builder.Services.AddSingleton<IEventPublisher>(sp =>
    new RabbitMqPublisher(rabbitMqSettings));

// Add Cors etc.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();

public class AddHeaderParameterOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        // Добавляем заголовок X-User-Id
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "X-User-Id",
            In = ParameterLocation.Header,
            Description = "ID user",
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string" // или другой тип
            }
        });
    }
}
