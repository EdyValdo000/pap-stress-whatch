﻿using pap.Database;
using pap.Model;
using SQLite;

namespace pap.Repositore;
public class GSRDataRepository: ConectionDatabase
{
    public GSRDataRepository()
    {
        
    }

    // Create
    public Task<int> SaveGSRDataAsync(GSRData data)
    {
        return database.InsertAsync(data);
    }

    // Read - Todas as medições
    public Task<List<GSRData>> GetAllGSRDataAsync()
    {
        return database.Table<GSRData>().ToListAsync();
    }

    // Read - Buscar medições por sessão
    public Task<List<GSRData>> GetDataBySessionIdAsync(int sessionId)
    {
        return database.Table<GSRData>().Where(d => d.SessionId == sessionId).ToListAsync();
    }

    // Update
    public Task<int> UpdateGSRDataAsync(GSRData data)
    {
        return database.UpdateAsync(data);
    }

    // Delete
    public Task<int> DeleteGSRDataAsync(GSRData data)
    {
        return database.DeleteAsync(data);
    }

    // Extra 1 - Obter última medição de GSR em uma sessão
    public Task<GSRData> GetLastReadingBySessionAsync(int sessionId)
    {
        return database.Table<GSRData>().Where(d => d.SessionId == sessionId)
            .OrderByDescending(d => d.Timestamp).FirstOrDefaultAsync();
    }

    // Extra 2 - Obter média de GSR de uma sessão
    public Task<double?> GetAverageGSRBySessionAsync(int sessionId)
    {
        return database.ExecuteScalarAsync<double?>("SELECT AVG(GSR) FROM GSRData WHERE SessionId = ?", sessionId);
    }

    // Extra 3 - Obter maior condutância registrada em uma sessão
    public Task<double?> GetMaxGSRBySessionAsync(int sessionId)
    {
        return database.ExecuteScalarAsync<double?>("SELECT MAX(GSR) FROM GSRData WHERE SessionId = ?", sessionId);
    }
}
