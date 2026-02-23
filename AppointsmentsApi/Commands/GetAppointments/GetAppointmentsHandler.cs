using AppointsmentsApi.Models;
using AppointsmentsApi.Models.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AppointsmentsApi.Commands.GetAppointments;

public class GetAppointmentsHandler(AppointmentContext context): IRequestHandler<GetAppointmentsQuery, List<Appointment>>
{
    private readonly AppointmentContext _context = context;

    public async Task<List<Appointment>> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Appointments.AsNoTracking().ToListAsync(cancellationToken);
    }
}
