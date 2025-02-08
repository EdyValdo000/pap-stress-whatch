using SQLite;
namespace pap.Model;
public class TemperatureData
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int SessionId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public double Temperature { get; set; } // Temperatura em °C
}