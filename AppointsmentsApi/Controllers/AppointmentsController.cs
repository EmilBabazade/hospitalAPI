using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppointsmentsApi.Models;
using AppointsmentsApi.Models.Data;
using AppointsmentsApi.Services;
using Grpc.Net.Client;
using AppointsmentsApi.Protos;
using MassTransit;
using Messages;

namespace AppointsmentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppointmentContext _context;
        private readonly PatientsApiClient _patientsApiClient;
        private readonly DoctorsApiClient _doctorsApiClient;
        private readonly IConfiguration _config;
        private readonly IPublishEndpoint _publishEndpoint;

        public AppointmentsController(AppointmentContext context, PatientsApiClient patientsApiClient, 
            DoctorsApiClient doctorsApiClient, IConfiguration config, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _patientsApiClient = patientsApiClient;
            _doctorsApiClient = doctorsApiClient;
            _config = config;
            _publishEndpoint = publishEndpoint;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDetailsDTO>> GetAppointment(Guid id, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            // get documents
            using var channel = GrpcChannel.ForAddress(_config["GrpcEndpoints:DocumentService"]);
            var client = new DocumentSearch.DocumentSearchClient(channel);
            var documents = await client.GetAllAsync(new PatientId {Id = appointment.PatientId.ToString()}, cancellationToken: cancellationToken);
            
            return new AppointmentDetailsDTO(appointment, documents);
        }

        // PUT: api/Appointments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(Guid id, Appointment appointment)
        {
            if (id != appointment.AppointmentId)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var msg = new AppointmentCreatedMessage(appointment.AppointmentId, appointment.PatientId, appointment.DoctorId, appointment.Slot.Start);
            await _publishEndpoint.Publish(msg);
            if (Random.Shared.Next() % 2 == 0)
            {
                await _publishEndpoint.Publish(msg); // simulate duplicate messages
            }
            Console.WriteLine($"Sent:\n {msg}");

            return CreatedAtAction("GetAppointment", new { id = appointment.AppointmentId }, appointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(Guid id)
        {
            return _context.Appointments.Any(e => e.AppointmentId == id);
        }
    }
}