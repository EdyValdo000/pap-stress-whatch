using SQLite;

namespace pap.Model;
public class MonitoringSession
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; }
}