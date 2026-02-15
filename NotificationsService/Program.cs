using MassTransit;
using NotificationsService.MessageConsumers;
using NotificationsService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddMassTransit(opts =>
{
    opts.AddConsumer<AppointmnetCreatedConsumer>();
    opts.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint("appointment_created_queue", e =>
        {
            e.ConfigureConsumer<AppointmnetCreatedConsumer>(context);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
