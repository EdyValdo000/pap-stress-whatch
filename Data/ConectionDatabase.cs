using NewPap.Model;
using SQLite;

namespace NewPap.Database;

public class ConectionDatabase
{
    public readonly SQLiteAsyncConnection database;

    public ConectionDatabase()
    {
        var local = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pap.db3");
        database = new SQLiteAsyncConnection(local);
        
        database.CreateTableAsync<User>().Wait();
        database.CreateTableAsync<SensorData>().Wait();
    }       
}