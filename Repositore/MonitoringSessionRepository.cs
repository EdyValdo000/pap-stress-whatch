using SQLite;
using pap.Model;
using pap.Database;

namespace pap.Repositore;
public class MonitoringSessionRepository: ConectionDatabase
{
    public MonitoringSessionRepository()
    {
    }

    // Create
    public Task<int> SaveSessionAsync(MonitoringSession session)
    {
        return database.InsertAsync(session);
    }

    // Read - Todas as sessões
    public Task<List<MonitoringSession>> GetSessionsAsync()
    {
        return database.Table<MonitoringSession>().ToListAsync();
    }

    // Read - Buscar sessão por ID
    public Task<MonitoringSession> GetSessionByIdAsync(int id)
    {
        return database.Table<MonitoringSession>().Where(s => s.Id == id).FirstOrDefaultAsync();
    }

    // Update
    public Task<int> UpdateSessionAsync(MonitoringSession session)
    {
        return database.UpdateAsync(session);
    }

    // Delete
    public Task<int> DeleteSessionAsync(MonitoringSession session)
    {
        return database.DeleteAsync(session);
    }

    // Extra 1 - Buscar sessões ativas (sem EndTime)
    public Task<List<MonitoringSession>> GetActiveSessionsAsync()
    {
        return database.Table<MonitoringSession>().Where(s => s.EndTime == null).ToListAsync();
    }

    // Extra 2 - Encerrar uma sessão
    public Task<int> EndSessionAsync(int sessionId)
    {
        return database.ExecuteAsync("UPDATE MonitoringSession SET EndTime = ? WHERE Id = ?", DateTime.UtcNow, sessionId);
    }

    // Extra 3 - Obter última sessão do usuário
    public Task<MonitoringSession> GetLastSessionByUserAsync(int userId)
    {
        return database.Table<MonitoringSession>().Where(s => s.UserId == userId)
            .OrderByDescending(s => s.StartTime).FirstOrDefaultAsync();
    }
}
