using SQLite;
namespace pap.Model;
public class GSRData
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int SessionId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public double GSR { get; set; } // Resistência da pele (Ohms)
}