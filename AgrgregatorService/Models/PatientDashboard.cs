namespace AgrgregatorService.Models;

public class PatientDashboard
{
    public PatientDto? Patient { get; set; }
    public List<AppointmentDto>? Appointments { get; set; }
    // Additional details
}
public class PatientDto
{
    public Guid Id { get; set; }
    public string Fullname { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
}
public class AppointmentDto
{
    public Guid Id { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string DoctorName { get; set; } = string.Empty;
}
