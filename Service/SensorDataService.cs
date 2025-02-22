using pap.Model;
using pap.Repositore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pap.Service;

public class SensorDataService
{
    private readonly SensorDataRepository repository;

    public SensorDataService(SensorDataRepository repository)
    {
        this.repository = repository;
    }
    
    // Retorna todos os SensorData com o mesmo UserId
    public Task<List<SensorData>> GetSensorDataByUserId(int userId)
        => repository.GetSensorDataByUserIdAsync(userId);

    // Salvar dados dos sensores
    public Task<int> Save(SensorData sensorData) => repository.SaveSensorDataAsync(sensorData);

    // Obter todos os dados dos sensores
    public Task<List<SensorData>> GetAll() => repository.GetAllSensorDataAsync();

    // Obter dados dos sensores por ID
    public Task<SensorData> GetById(int id) => repository.GetSensorDataByIdAsync(id);

    // Atualizar dados dos sensores
    public Task<int> Update(SensorData sensorData) => repository.UpdateSensorDataAsync(sensorData);

    // Excluir dados dos sensores
    public Task<int> Delete(SensorData sensorData) => repository.DeleteSensorDataAsync(sensorData);

    // Extra 1 - Obter dados dos sensores por UserId
    public Task<List<SensorData>> GetByUserId(int userId) => repository.GetSensorDataByUserIdAsync(userId);

    // Extra 2 - Obter dados dos sensores com nível de estresse acima de um valor
    public Task<List<SensorData>> GetByStressLevel(int minStressLevel) => repository.GetSensorDataByStressLevelAsync(minStressLevel);

    // Extra 3 - Obter os últimos dados dos sensores (limit 10)
    public Task<List<SensorData>> GetLastSensorData(int limit = 10) => repository.GetLastSensorDataAsync(limit);

    // Extra 4 - Contar o número de registros de dados dos sensores
    public Task<int> GetCount() => repository.GetSensorDataCountAsync();

    // Extra 5 - Obter dados dos sensores em um intervalo de tempo
    public Task<List<SensorData>> GetByTimeRange(DateTime startTime, DateTime endTime) => repository.GetSensorDataByTimeRangeAsync(startTime, endTime);
}