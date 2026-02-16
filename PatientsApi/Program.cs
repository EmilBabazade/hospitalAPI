var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

app.MapGet("/", () =>
{
    List<Patient> patients = [
        Patient.NewRandomPatient("987e6543-e21b-34d3-c456-426614174111"),
        Patient.NewRandomPatient(),
        Patient.NewRandomPatient(),
        Patient.NewRandomPatient(),
        Patient.NewRandomPatient(),
        Patient.NewRandomPatient(),
        Patient.NewRandomPatient(),
        Patient.NewRandomPatient(),
    ];
    return patients;
})
.WithName("GetPatients");

app.Run();
public record Patient(
    Guid PatientId,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Gender,
    string ContactNumber,
    string Email
)
{
    private static readonly string[] MaleFirstNames =
    {
        "John", "Michael", "David", "James", "Daniel",
        "Robert", "Thomas", "Mark"
    };

    private static readonly string[] FemaleFirstNames =
    {
        "Anna", "Emma", "Sarah", "Laura", "Emily",
        "Olivia", "Sophia", "Maria"
    };

    private static readonly string[] LastNames =
    {
        "Smith", "Johnson", "Brown", "Taylor",
        "Anderson", "Wilson", "Martinez", "Garcia"
    };

    private static readonly Random _random = new();

    public static Patient NewRandomPatient()
    {
        var gender = _random.Next(0, 2) == 0 ? "Male" : "Female";

        var firstName = gender == "Male"
            ? MaleFirstNames[_random.Next(MaleFirstNames.Length)]
            : FemaleFirstNames[_random.Next(FemaleFirstNames.Length)];

        var lastName = LastNames[_random.Next(LastNames.Length)];

        // age between 1 and 100
        var age = _random.Next(1, 101);
        var dateOfBirth = DateTime.Today
            .AddYears(-age)
            .AddDays(-_random.Next(0, 365));

        var email = $"{firstName}.{lastName}{_random.Next(100, 999)}@example.com"
            .ToLowerInvariant();

        var contactNumber = $"+1-555-{_random.Next(100, 999)}-{_random.Next(1000, 9999)}";

        return new Patient(
            Guid.NewGuid(),
            firstName,
            lastName,
            dateOfBirth,
            gender,
            contactNumber,
            email
        );
    }

    public static Patient NewRandomPatient(string guid)
    {
        var patient = NewRandomPatient();
        return patient with { PatientId = Guid.Parse(guid) };
    }
}
