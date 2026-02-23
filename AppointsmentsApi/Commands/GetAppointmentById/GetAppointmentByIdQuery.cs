using AppointsmentsApi.Models;
using MediatR;

namespace AppointsmentsApi.Commands.GetAppointmentById;

public record GetAppointmentByIdQuery(string Id): IRequest<AppointmentDetailsDTO>;
