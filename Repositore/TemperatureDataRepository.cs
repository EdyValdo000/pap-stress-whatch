using SQLite;
using pap.Model;
using pap.Database;

namespace pap.Repositore;
public class TemperatureDataRepository: ConectionDatabase
{
    public TemperatureDataRepository()
    {
 
    }

    // Create
    public Task<int> SaveTemperatureDataAsync(TemperatureData data)
    {
        return database.InsertAsync(data);
    }

    // Read - Todas as medições
    public Task<List<TemperatureData>> GetAllTemperatureDataAsync()
    {
        return database.Table<TemperatureData>().ToListAsync();
    }

    // Read - Buscar medições por sessão
    public Task<List<TemperatureData>> GetDataBySessionIdAsync(int sessionId)
    {
        return database.Table<TemperatureData>().Where(d => d.SessionId == sessionId).ToListAsync();
    }

    // Update
    public Task<int> UpdateTemperatureDataAsync(TemperatureData data)
    {
        return database.UpdateAsync(data);
    }

    // Delete
    public Task<int> DeleteTemperatureDataAsync(TemperatureData data)
    {
        return database.DeleteAsync(data);
    }

    // Extra 1 - Obter última medição de temperatura em uma sessão
    public Task<TemperatureData> GetLastReadingBySessionAsync(int sessionId)
    {
        return database.Table<TemperatureData>().Where(d => d.SessionId == sessionId)
            .OrderByDescending(d => d.Timestamp).FirstOrDefaultAsync();
    }

    // Extra 2 - Obter temperatura média de uma sessão
    public Task<double?> GetAverageTemperatureBySessionAsync(int sessionId)
    {
        return database.ExecuteScalarAsync<double?>("SELECT AVG(Temperature) FROM TemperatureData WHERE SessionId = ?", sessionId);
    }

    // Extra 3 - Obter temperatura máxima registrada em uma sessão
    public Task<double?> GetMaxTemperatureBySessionAsync(int sessionId)
    {
        return database.ExecuteScalarAsync<double?>("SELECT MAX(Temperature) FROM TemperatureData WHERE SessionId = ?", sessionId);
    }
}
