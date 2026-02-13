using AppointsmentsApi.Protos;

namespace AppointsmentsApi.Models;

public class AppointmentDetailsDTO : Appointment
{
    public DocumentList DocumentList { get; set; }

    public AppointmentDetailsDTO(Appointment appointment, DocumentList documentList)
    {
        AppointmentId = appointment.AppointmentId;
        PatientId = appointment.PatientId;
        DoctorId = appointment.DoctorId;
        Slot = appointment.Slot;
        Location = appointment.Location;
        Purpose = appointment.Purpose;
        DocumentList = documentList;
    }
}