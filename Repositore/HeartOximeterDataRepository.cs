using SQLite;
using pap.Model;

namespace pap.Repositore;
public class HeartOximeterDataRepository
{
    private readonly SQLiteAsyncConnection _database;

    public HeartOximeterDataRepository(SQLiteAsyncConnection database)
    {
        _database = database;
    }

    // Create
    public Task<int> SaveHeartOximeterDataAsync(HeartOximeterData data)
    {
        return _database.InsertAsync(data);
    }

    // Read - Todas as medições
    public Task<List<HeartOximeterData>> GetAllHeartOximeterDataAsync()
    {
        return _database.Table<HeartOximeterData>().ToListAsync();
    }

    // Read - Buscar medições por sessão
    public Task<List<HeartOximeterData>> GetDataBySessionIdAsync(int sessionId)
    {
        return _database.Table<HeartOximeterData>().Where(d => d.SessionId == sessionId).ToListAsync();
    }

    // Update
    public Task<int> UpdateHeartOximeterDataAsync(HeartOximeterData data)
    {
        return _database.UpdateAsync(data);
    }

    // Delete
    public Task<int> DeleteHeartOximeterDataAsync(HeartOximeterData data)
    {
        return _database.DeleteAsync(data);
    }

    // Extra 1 - Obter última medição de uma sessão
    public Task<HeartOximeterData> GetLastReadingBySessionAsync(int sessionId)
    {
        return _database.Table<HeartOximeterData>().Where(d => d.SessionId == sessionId)
            .OrderByDescending(d => d.Timestamp).FirstOrDefaultAsync();
    }

    // Extra 2 - Obter média dos batimentos cardíacos de uma sessão
    public Task<double?> GetAverageHeartRateBySessionAsync(int sessionId)
    {
        return _database.ExecuteScalarAsync<double?>("SELECT AVG(HeartRate) FROM HeartOximeterData WHERE SessionId = ?", sessionId);
    }

    // Extra 3 - Obter a maior saturação de oxigênio registrada em uma sessão
    public Task<double?> GetMaxSpO2BySessionAsync(int sessionId)
    {
        return _database.ExecuteScalarAsync<double?>("SELECT MAX(SpO2) FROM HeartOximeterData WHERE SessionId = ?", sessionId);
    }
}
