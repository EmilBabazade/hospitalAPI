using AppointsmentsApi.Models;
using AppointsmentsApi.Models.Data;
using MassTransit;
using MediatR;
using Messages;

namespace AppointsmentsApi.Commands.CreateAppointment;

public class CreateAppointmentHandler(
    AppointmentContext context, 
    IPublishEndpoint publishEndpoint
    ) : IRequestHandler<CreateAppointmentCommand, Appointment>
{
    private readonly AppointmentContext _context = context;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task<Appointment> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var newAppointment = new Appointment
        {
            AppointmentId = Guid.NewGuid(),
            DoctorId = request.DoctorId,
            PatientId = request.PatientId,
            Location = request.Location,
            Slot = request.Slot,
            Purpose = request.Purpose
        };
        _context.Add(newAppointment);
        await _context.SaveChangesAsync(cancellationToken);

        //Perform post-creation hand-off to services bus.
        await _publishEndpoint.Publish<AppointmentCreatedMessage>(new
        {
            newAppointment.AppointmentId,
            newAppointment.PatientId,
            newAppointment.DoctorId,
            newAppointment.Slot.Start,
            DateTime.UtcNow,
            MessageId = newAppointment.AppointmentId
        }, cancellationToken);
        return newAppointment;
    }
}