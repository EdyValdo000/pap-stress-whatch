using SQLite;

namespace pap.Model;
public class HeartOximeterData
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int SessionId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public int? HeartRate { get; set; } // BPM
    public double? SpO2 { get; set; }   // Saturação de oxigênio (%)
}