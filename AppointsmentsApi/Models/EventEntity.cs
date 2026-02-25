using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointsmentsApi.Models;

public class EventEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string EventType { get; set; }
    public DateTime EventTimeStamp { get; set; }
    [Column(TypeName = "jsonb")]
    public string Payload { get; set; }
}
