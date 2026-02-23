using AgrgregatorService.Models;

namespace AgrgregatorService.Services;

public class AppointmentsServiceClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    public async Task<List<AppointmentDto>?>
        GetAppointmentsAsync(string patientId)
    {
        var appointments = await
            _httpClient.GetFromJsonAsync<List<AppointmentDto>?>
                ($"api/appointmentsforpatient/{patientId}");
        return appointments;
    }
}

public class PatientServiceClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<PatientDto?> GetPatientAsync(string patientId)
    {
        return await _httpClient.GetFromJsonAsync<PatientDto>($"/patients/{patientId}");
    }
}