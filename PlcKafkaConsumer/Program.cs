using PlcKafkaConsumer.EventHandlers;
using PlcKafkaProducer.Models;
using PlcKafkaLibrary;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddKafkaConnection(builder.Configuration);
builder.Services.AddKafkaConsumer<string, User, UserHandler>();
builder.Services.AddKafkaConsumer<string, Drink, DrinkHandler>();

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
