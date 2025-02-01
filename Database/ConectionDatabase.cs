using SQLite;
namespace pap.Database;

public class ConectionDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public ConectionDatabase()
    {
        var local = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pap.db3");
        _database = new SQLiteAsyncConnection(local);
        _database.CreateTableAsync<Users>().Wait();
    }

    public Task<int> SaveUsers(Users users)
    {
        return _database.InsertAllAsync((System.Collections.IEnumerable)users);
    }
}