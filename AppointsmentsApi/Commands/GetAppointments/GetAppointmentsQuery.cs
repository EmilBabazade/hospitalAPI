using AppointsmentsApi.Models;
using MediatR;

namespace AppointsmentsApi.Commands.GetAppointments;

public record GetAppointmentsQuery(): IRequest<List<Appointment>>;
