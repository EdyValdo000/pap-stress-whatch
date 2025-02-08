using SQLite;
namespace pap.Model;
public class AIRecommendation
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int SessionId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Recommendation { get; set; } = string.Empty; // Sugestão gerada pela IA
}