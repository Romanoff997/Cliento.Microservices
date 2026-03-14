// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Threading.Channels;

Console.WriteLine("Hello, World!");
var factory = new ConnectionFactory { HostName = "rabbitHost" };
var _connection = factory.CreateConnection();// CreateConnection();
var _channel = _connection.CreateModel();