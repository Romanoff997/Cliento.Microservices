using Cliento.Microservices.NotificationService.Consumers;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
builder.Services.AddHostedService(sp => new RabbitMqConsumer(builder.Configuration.GetValue<string>("RabbitMq:Host")));
var app = builder.Build();
app.Run();
