using Confluent.Kafka;

using PlcKafkaLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddKafkaProducer<string, object>(p =>
{
    p.BootstrapServers = "localhost:9092";
    p.SaslMechanism = SaslMechanism.Plain;
    p.SecurityProtocol = SecurityProtocol.SaslPlaintext;
    p.SaslUsername = "admin";
    p.SaslPassword = "admin-secret";
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

app.MapControllers();

app.Run();
