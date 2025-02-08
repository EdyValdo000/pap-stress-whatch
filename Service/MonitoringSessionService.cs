using pap.Model;
using pap.Repositore;

namespace pap.Service;
public class MonitoringSessionService
{
    private readonly MonitoringSessionRepository _repositore;

    public MonitoringSessionService(MonitoringSessionRepository repositore)
    {
        _repositore = repositore;
    }

    public Task<int> Save(MonitoringSession session) => _repositore.SaveSessionAsync(session);
    public Task<MonitoringSession> GetById(int id) => _repositore.GetSessionByIdAsync(id);
    public Task<List<MonitoringSession>> GetAll() => _repositore.GetSessionsAsync();
    public Task<int> Update(MonitoringSession session) => _repositore.UpdateSessionAsync(session);
    public Task<int> Delete(MonitoringSession session) => _repositore.DeleteSessionAsync(session);
}
