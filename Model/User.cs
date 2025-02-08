using SQLite;

namespace pap.Model;
public class User
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty; // "Masculino", "Feminino" ou "Outro"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}