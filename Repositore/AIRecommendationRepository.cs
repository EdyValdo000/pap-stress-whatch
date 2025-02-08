using SQLite;
using pap.Model;

namespace pap.Repositore;
public class AIRecommendationRepository
{
    private readonly SQLiteAsyncConnection _database;

    public AIRecommendationRepository(SQLiteAsyncConnection database)
    {
        _database = database;
    }

    // Create
    public Task<int> SaveRecommendationAsync(AIRecommendation recommendation)
    {
        return _database.InsertAsync(recommendation);
    }

    // Read - Todas as recomendações
    public Task<List<AIRecommendation>> GetAllRecommendationsAsync()
    {
        return _database.Table<AIRecommendation>().ToListAsync();
    }

    // Read - Buscar recomendações por sessão
    public Task<List<AIRecommendation>> GetRecommendationsBySessionIdAsync(int sessionId)
    {
        return _database.Table<AIRecommendation>().Where(r => r.SessionId == sessionId).ToListAsync();
    }

    // Update
    public Task<int> UpdateRecommendationAsync(AIRecommendation recommendation)
    {
        return _database.UpdateAsync(recommendation);
    }

    // Delete
    public Task<int> DeleteRecommendationAsync(AIRecommendation recommendation)
    {
        return _database.DeleteAsync(recommendation);
    }

    // Extra 1 - Obter a última recomendação feita na sessão
    public Task<AIRecommendation> GetLastRecommendationBySessionAsync(int sessionId)
    {
        return _database.Table<AIRecommendation>().Where(r => r.SessionId == sessionId)
            .OrderByDescending(r => r.Timestamp).FirstOrDefaultAsync();
    }

    // Extra 2 - Obter recomendações feitas nas últimas 24 horas
    public Task<List<AIRecommendation>> GetRecentRecommendationsAsync()
    {
        DateTime limit = DateTime.UtcNow.AddDays(-1);
        return _database.Table<AIRecommendation>().Where(r => r.Timestamp >= limit).ToListAsync();
    }

    // Extra 3 - Contar número de recomendações feitas em uma sessão
    public Task<int> GetRecommendationCountBySessionAsync(int sessionId)
    {
        return _database.Table<AIRecommendation>().Where(r => r.SessionId == sessionId).CountAsync();
    }
}
