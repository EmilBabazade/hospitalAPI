using AppointsmentsApi.Models;
using AppointsmentsApi.Models.Data;
using AppointsmentsApi.Protos;
using Grpc.Net.Client;
using MediatR;

namespace AppointsmentsApi.Commands.GetAppointmentById;

public class GetAppointmentsByIdHandler(
    AppointmentContext context, 
    IConfiguration configuration) : IRequestHandler<GetAppointmentByIdQuery, AppointmentDetailsDTO>
{
    private readonly AppointmentContext _context = context;
    private readonly IConfiguration _configuration = configuration;

    public async Task<AppointmentDetailsDTO> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.Id);
        var appointment = await _context.Appointments.FindAsync(id, cancellationToken, cancellationToken);

        if (appointment == null)
        {
            return null;
        }

        // get documents
        using var channel = GrpcChannel.ForAddress(_configuration["GrpcEndpoints:DocumentService"]);
        var client = new DocumentSearch.DocumentSearchClient(channel);
        var documents = await client.GetAllAsync(new PatientId { Id = appointment.PatientId.ToString() }, cancellationToken: cancellationToken);

        return new AppointmentDetailsDTO(appointment, documents);
    }
}
