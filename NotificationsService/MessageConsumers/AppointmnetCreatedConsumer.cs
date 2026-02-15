using MassTransit;
using Messages;

namespace NotificationsService.MessageConsumers;

public class AppointmnetCreatedConsumer : IConsumer<AppointmentCreated>
{
    public async Task Consume(ConsumeContext<AppointmentCreated> context)
    {
        var message = context.Message;
        Console.WriteLine($"Recieved:\n {message}");

        // get data from doctor and patient services for doctor and patient...
        await Task.Delay(1000);

        Console.WriteLine("Sending Email...");
    }
}
