using pap.Model;
using pap.Repositore;

namespace pap.Service;
public class AIRecommendationService
{
    private readonly AIRecommendationRepository _repositore;

    public AIRecommendationService(AIRecommendationRepository repositore)
    {
        _repositore = repositore;
    }

    public Task<int> Save(AIRecommendation recommendation) => _repositore.SaveRecommendationAsync(recommendation);
    public Task<AIRecommendation> GetById(int id) => _repositore.GetRecommendationsBySessionIdAsync(id).ContinueWith(t => t.Result.FirstOrDefault()!);
    public Task<List<AIRecommendation>> GetAll() => _repositore.GetAllRecommendationsAsync();
    public Task<int> Update(AIRecommendation recommendation) => _repositore.UpdateRecommendationAsync(recommendation);
    public Task<int> Delete(AIRecommendation recommendation) => _repositore.DeleteRecommendationAsync(recommendation);
}