using System.Text.Json.Serialization;

namespace Messages;

public class AppointmentCreatedMessage
{
    [JsonInclude]
    public Guid Id { get; private set; }
    [JsonInclude]
    public Guid AppointmentId { get; private set; }
    [JsonInclude]
    public Guid PatientId { get; private set; }
    [JsonInclude]
    public Guid DoctorId { get; private set; }
    [JsonInclude]
    public DateTime AppointmentDate { get; private set; }
    [JsonInclude]
    public DateTime TimeStamp { get; private set; }

    public AppointmentCreatedMessage(Guid appointmentId, Guid patientId, Guid doctorId, DateTime appointmentDate)
    {
        AppointmentId = appointmentId;
        PatientId = patientId;
        DoctorId = doctorId;
        AppointmentDate = appointmentDate;
        TimeStamp = DateTime.UtcNow;
        Id = Guid.NewGuid();
    }

    public AppointmentCreatedMessage() // necessary for serialization
    {
    }

    public override string ToString()
    {
        var str = $"Id = {Id}\nAppointmentId = {AppointmentId}\nPatientId = {PatientId}\nDoctorId = {DoctorId}\nAppointmentDate = {AppointmentDate}\n------------------";
        return str;
    }
}
