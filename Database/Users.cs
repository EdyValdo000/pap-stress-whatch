using SQLite;
namespace pap.Database;

public class Users
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}