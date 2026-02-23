using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppointsmentsApi.Models;
using AppointsmentsApi.Models.Data;
using AppointsmentsApi.Services;
using Grpc.Net.Client;
using AppointsmentsApi.Protos;
using MassTransit;
using Messages;
using MediatR;
using AppointsmentsApi.Commands.CreateAppointment;
using AppointsmentsApi.Commands.GetAppointmentById;
using AppointsmentsApi.Commands.GetAppointments;

namespace AppointsmentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController(
        AppointmentContext context, 
        PatientsApiClient patientsApiClient,
        DoctorsApiClient doctorsApiClient, 
        IConfiguration config, 
        IPublishEndpoint publishEndpoint,
        IMediator mediator
        ) : ControllerBase
    {
        private readonly AppointmentContext _context = context;
        private readonly PatientsApiClient _patientsApiClient = patientsApiClient;
        private readonly DoctorsApiClient _doctorsApiClient = doctorsApiClient;
        private readonly IConfiguration _config = config;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IMediator _mediator = mediator;

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _mediator.Send(new GetAppointmentsQuery());
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDetailsDTO>> GetAppointment(string id, CancellationToken cancellationToken)
        {
            var apppointment = await _mediator.Send(new GetAppointmentByIdQuery(id), cancellationToken);

            if (apppointment == null)
            {
                return NotFound();
            }
            
            return apppointment;
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
        public async Task<ActionResult<Appointment>> PostAppointment(CreateAppointmentCommand createAppointmentCommand)
        {
            var appointment = await _mediator.Send(createAppointmentCommand);

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

        [HttpGet("GetAppointmentsByPatientId/{patientId}")]
        public async Task<ActionResult<IEnumerable<AppointmentByPatientId>>>
        GetAppointmentsByPatientId(Guid patientId)
        {
            // Get appointments for a patient
            var appointments = await _context.Appointments.Where(a =>
                a.PatientId == patientId)
            .Select(q => new AppointmentByPatientId(q.AppointmentId,
                string.Empty, q.Slot.Start))
            .ToListAsync();
            // Get doctor details for each appointment in parallel
            var tasks = appointments.Select(async appointment =>
            {
                var doctor = await _doctorsApiClient
                    .GetDoctorAsync(appointment.AppointmentId);
                appointment.DoctorName = doctor.LastName;
            });
            await Task.WhenAll(tasks); 
            return appointments;
        }
        // New DTO for this operation
        public class AppointmentByPatientId(Guid appointmentId,
            string doctorName, DateTime date)
        {
            public Guid AppointmentId { get; set; } = appointmentId;
            public string DoctorName { get; set; } = doctorName;
            public DateTime Date { get; set; } = date;
        }
    }
}