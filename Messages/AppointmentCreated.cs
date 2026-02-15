namespace Messages;

public class AppointmentCreated
{
    public Guid AppointmentId { get; set; }
    public Guid PatientId { get; set; }
    public Guid DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }

    public override string ToString()
    {
        var str = $"AppointmentId = {AppointmentId}\nPatientId = {PatientId}\nDoctorId = {DoctorId}\nAppointmentDate = {AppointmentDate}\n------------------";
        return str;
    }
}
