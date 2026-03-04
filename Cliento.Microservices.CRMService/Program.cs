using Cliento.Microservices.CRMService.Cache;
using Cliento.Microservices.CRMService.Data;
using Cliento.Microservices.Shared.Messaging;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CrmDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("Redis:Connection")));

builder.Services.AddSingleton<RedisSessionCache>();

// RabbitMQ publisher (host from config)
builder.Services.AddSingleton<IEventPublisher>(sp =>
    new RabbitMqPublisher(builder.Configuration.GetValue<string>("RabbitMq:Host")));

// Add Cors etc.
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
