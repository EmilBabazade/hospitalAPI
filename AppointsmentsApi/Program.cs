using AppointsmentsApi.Models.Data;
using AppointsmentsApi.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppointmentContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString(
        "DefaultConnection")));
builder.Services.AddDbContext<EventStoreDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString(
        "EventStoreDatabase")));
// apiclients
// Register PatientsApiClient
builder.Services.AddHttpClient<PatientsApiClient>(client =>
{
    client.BaseAddress = new
        Uri(builder.Configuration["ApiEndpoints:PatientsApi"]);
});
// Register DoctorsApiClient
builder.Services.AddHttpClient<DoctorsApiClient>(client =>
{
    client.BaseAddress = new
        Uri(builder.Configuration["ApiEndpoints:DoctorsApi"]);
});
// rabbitmq
builder.Services.AddMassTransit(opts => opts.UsingRabbitMq());
// mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();