using System.Net.Http.Json;
using System.Text.Json;

namespace HealthcareDashboard.Services;

public record Patient(
    Guid PatientId,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Gender,
    string ContactNumber,
    string Email
);

public class PatientServiceClient
{
    private readonly HttpClient _httpClient;

    public PatientServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Patient?> GetPatientAsync(string patientId)
    {
        var response = await _httpClient.GetAsync("/");

        var json = await response.Content.ReadAsStringAsync();

        Console.WriteLine("RAW RESPONSE:");
        Console.WriteLine(json);

        response.EnsureSuccessStatusCode();

        var patients = JsonSerializer.Deserialize<List<Patient>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return patients?
            .FirstOrDefault(x => x.PatientId == Guid.Parse(patientId));
    }

}