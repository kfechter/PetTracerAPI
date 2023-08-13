using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PetTracerAPI.Messaging;
using PetTracerAPI.Models;
using PetTracerAPI.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var firebaseAPIKey = Environment.GetEnvironmentVariable("FirebaseKey");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add MongoDB Support
builder.Services.Configure<PetTracerDatabaseSettings>(
    builder.Configuration.GetSection("PetTracerDatabase"));

builder.Services.AddSingleton<UsersService>();
builder.Services.AddSingleton<PetsService>();
builder.Services.AddSingleton<NotificationsService>();

builder.Services.AddTransient<RabbitMQConsumer>();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the firebase JWTBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{firebaseAPIKey}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{firebaseAPIKey}",
            ValidateAudience = true,
            ValidAudience = firebaseAPIKey,
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();


var factory = new ConnectionFactory
{
    HostName = Environment.GetEnvironmentVariable("RabbitMQHost"),
    Port = 5672,
    VirtualHost = Environment.GetEnvironmentVariable("VHostName"),
    UserName = Environment.GetEnvironmentVariable("RabbitUser"),
    Password = Environment.GetEnvironmentVariable("RabbitPassword")
};
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(Environment.GetEnvironmentVariable("TagQueueName"));

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var consumerService = app.Services.GetRequiredService<RabbitMQConsumer>();
    consumerService.ConsumeMessage(message);
};

app.Run();