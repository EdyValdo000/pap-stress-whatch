using SQLite;
using pap.Model;
using pap.Database;

namespace pap.Repositore;
public class HeartOximeterDataRepository: ConectionDatabase
{
    public HeartOximeterDataRepository()
    {

    }

    // Create
    public Task<int> SaveHeartOximeterDataAsync(HeartOximeterData data)
    {
        return database.InsertAsync(data);
    }

    // Read - Todas as medições
    public Task<List<HeartOximeterData>> GetAllHeartOximeterDataAsync()
    {
        return database.Table<HeartOximeterData>().ToListAsync();
    }

    // Read - Buscar medições por sessão
    public Task<List<HeartOximeterData>> GetDataBySessionIdAsync(int sessionId)
    {
        return database.Table<HeartOximeterData>().Where(d => d.SessionId == sessionId).ToListAsync();
    }

    // Update
    public Task<int> UpdateHeartOximeterDataAsync(HeartOximeterData data)
    {
        return database.UpdateAsync(data);
    }

    // Delete
    public Task<int> DeleteHeartOximeterDataAsync(HeartOximeterData data)
    {
        return database.DeleteAsync(data);
    }

    // Extra 1 - Obter última medição de uma sessão
    public Task<HeartOximeterData> GetLastReadingBySessionAsync(int sessionId)
    {
        return database.Table<HeartOximeterData>().Where(d => d.SessionId == sessionId)
            .OrderByDescending(d => d.Timestamp).FirstOrDefaultAsync();
    }

    // Extra 2 - Obter média dos batimentos cardíacos de uma sessão
    public Task<double?> GetAverageHeartRateBySessionAsync(int sessionId)
    {
        return database.ExecuteScalarAsync<double?>("SELECT AVG(HeartRate) FROM HeartOximeterData WHERE SessionId = ?", sessionId);
    }

    // Extra 3 - Obter a maior saturação de oxigênio registrada em uma sessão
    public Task<double?> GetMaxSpO2BySessionAsync(int sessionId)
    {
        return database.ExecuteScalarAsync<double?>("SELECT MAX(SpO2) FROM HeartOximeterData WHERE SessionId = ?", sessionId);
    }
}
