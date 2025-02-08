using SQLite;
using pap.Model;

namespace pap.Repositore;
public class MonitoringSessionRepository
{
    private readonly SQLiteAsyncConnection _database;

    public MonitoringSessionRepository(SQLiteAsyncConnection database)
    {
        _database = database;
    }

    // Create
    public Task<int> SaveSessionAsync(MonitoringSession session)
    {
        return _database.InsertAsync(session);
    }

    // Read - Todas as sessões
    public Task<List<MonitoringSession>> GetSessionsAsync()
    {
        return _database.Table<MonitoringSession>().ToListAsync();
    }

    // Read - Buscar sessão por ID
    public Task<MonitoringSession> GetSessionByIdAsync(int id)
    {
        return _database.Table<MonitoringSession>().Where(s => s.Id == id).FirstOrDefaultAsync();
    }

    // Update
    public Task<int> UpdateSessionAsync(MonitoringSession session)
    {
        return _database.UpdateAsync(session);
    }

    // Delete
    public Task<int> DeleteSessionAsync(MonitoringSession session)
    {
        return _database.DeleteAsync(session);
    }

    // Extra 1 - Buscar sessões ativas (sem EndTime)
    public Task<List<MonitoringSession>> GetActiveSessionsAsync()
    {
        return _database.Table<MonitoringSession>().Where(s => s.EndTime == null).ToListAsync();
    }

    // Extra 2 - Encerrar uma sessão
    public Task<int> EndSessionAsync(int sessionId)
    {
        return _database.ExecuteAsync("UPDATE MonitoringSession SET EndTime = ? WHERE Id = ?", DateTime.UtcNow, sessionId);
    }

    // Extra 3 - Obter última sessão do usuário
    public Task<MonitoringSession> GetLastSessionByUserAsync(int userId)
    {
        return _database.Table<MonitoringSession>().Where(s => s.UserId == userId)
            .OrderByDescending(s => s.StartTime).FirstOrDefaultAsync();
    }
}
