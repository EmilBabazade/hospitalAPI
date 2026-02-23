using AppointsmentsApi.Models;
using MediatR;

namespace AppointsmentsApi.Commands.CreateAppointment;

public record CreateAppointmentCommand(
    Guid DoctorId,
    Guid PatientId,
    Location Location,
    TimeSlot Slot,
    string Purpose
    ): IRequest<Appointment>;
