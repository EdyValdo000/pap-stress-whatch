using SQLite;
using pap.Model;
using pap.Database;

namespace pap.Repositore;
public class AIRecommendationRepository: ConectionDatabase
{

    public AIRecommendationRepository()
    {

    }

    // Create
    public Task<int> SaveRecommendationAsync(AIRecommendation recommendation)
    {
        return database.InsertAsync(recommendation);
    }

    // Read - Todas as recomendações
    public Task<List<AIRecommendation>> GetAllRecommendationsAsync()
    {
        return database.Table<AIRecommendation>().ToListAsync();
    }

    // Read - Buscar recomendações por sessão
    public Task<List<AIRecommendation>> GetRecommendationsBySessionIdAsync(int sessionId)
    {
        return database.Table<AIRecommendation>().Where(r => r.SessionId == sessionId).ToListAsync();
    }

    // Update
    public Task<int> UpdateRecommendationAsync(AIRecommendation recommendation)
    {
        return database.UpdateAsync(recommendation);
    }

    // Delete
    public Task<int> DeleteRecommendationAsync(AIRecommendation recommendation)
    {
        return database.DeleteAsync(recommendation);
    }

    // Extra 1 - Obter a última recomendação feita na sessão
    public Task<AIRecommendation> GetLastRecommendationBySessionAsync(int sessionId)
    {
        return database.Table<AIRecommendation>().Where(r => r.SessionId == sessionId)
            .OrderByDescending(r => r.Timestamp).FirstOrDefaultAsync();
    }

    // Extra 2 - Obter recomendações feitas nas últimas 24 horas
    public Task<List<AIRecommendation>> GetRecentRecommendationsAsync()
    {
        DateTime limit = DateTime.UtcNow.AddDays(-1);
        return database.Table<AIRecommendation>().Where(r => r.Timestamp >= limit).ToListAsync();
    }

    // Extra 3 - Contar número de recomendações feitas em uma sessão
    public Task<int> GetRecommendationCountBySessionAsync(int sessionId)
    {
        return database.Table<AIRecommendation>().Where(r => r.SessionId == sessionId).CountAsync();
    }
}
