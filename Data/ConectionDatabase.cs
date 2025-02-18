using pap.Model;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pap.Database
{
    public class ConectionDatabase
    {
        public readonly SQLiteAsyncConnection database;

        public ConectionDatabase()
        {
            var local = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pap.db3");
            database = new SQLiteAsyncConnection(local);
            
            database.CreateTableAsync<User>().Wait();
            database.CreateTableAsync<AIRecommendation>().Wait();
            database.CreateTableAsync<GSRData>().Wait();
            database.CreateTableAsync<HeartOximeterData>().Wait();
            database.CreateTableAsync<MonitoringSession>().Wait();
            database.CreateTableAsync<TemperatureData>().Wait();
        }       
    }
}