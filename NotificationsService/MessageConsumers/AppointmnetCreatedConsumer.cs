using MassTransit;
using Messages;
using System.Collections.Concurrent;

namespace NotificationsService.MessageConsumers;

public class AppointmnetCreatedConsumer : IConsumer<AppointmentCreatedMessage>
{
    private static readonly ConcurrentDictionary<Guid, DateTime> _lastProcessedTimestamps = [];
    // For this to work ConcurrencyLimit must be 1!
    private static readonly ConcurrentDictionary<Guid, bool> _processedMessageIds = [];
    public async Task Consume(ConsumeContext<AppointmentCreatedMessage> context)
    {
        var message = context.Message;
        var lastTimestamp = _lastProcessedTimestamps.GetOrAdd(message.AppointmentId, DateTime.MinValue);

        if(_processedMessageIds.ContainsKey(message.Id))
        {
            Console.WriteLine($"Message with id {message.Id} was already processed");
            return;
        }

        if (message.TimeStamp > lastTimestamp)
        {
            _lastProcessedTimestamps[message.AppointmentId] = message.TimeStamp;
            Console.WriteLine($"Recieved:\n {message}");
        }
        else
        {
            // handle out of order message
            throw new NotImplementedException();
        }

        // get data from doctor and patient services for doctor and patient details...
        await Task.Delay(1000);

        Console.WriteLine("Sending Email...");
        Console.WriteLine("---------------------------------------------------");
        _processedMessageIds[message.Id] = true;
    }
}
