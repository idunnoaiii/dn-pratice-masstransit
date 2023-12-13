using EventBus.Message.IntegrationEvent;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Producer.Api;
using Producer.Api.ServiceExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMasstransitConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();


app.MapPost("/send-notification", ([FromBody] NotificationEvent notificationEvent, [FromServices] IPublishEndpoint publishEndpoint) =>
{
    publishEndpoint.Publish<INotificationEvent>(notificationEvent);
    return Results.Accepted();
});

app.Run();
