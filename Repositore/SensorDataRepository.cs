using SQLite;
using pap.Model;
using pap.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pap.Repositore
{
    public class SensorDataRepository : ConectionDatabase
    {
        public SensorDataRepository()
        {            
        }

        // Create - Salvar dados dos sensores
        public Task<int> SaveSensorDataAsync(SensorData sensorData)
        {
            return database.InsertAsync(sensorData);
        }

        // Read - Obter todos os dados dos sensores
        public Task<List<SensorData>> GetAllSensorDataAsync()
        {
            return database.Table<SensorData>().ToListAsync();
        }

        // Read - Obter dados dos sensores por ID
        public Task<SensorData> GetSensorDataByIdAsync(int id)
        {
            return database.Table<SensorData>().Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        // Update - Atualizar dados dos sensores
        public Task<int> UpdateSensorDataAsync(SensorData sensorData)
        {
            return database.UpdateAsync(sensorData);
        }

        // Delete - Excluir dados dos sensores
        public Task<int> DeleteSensorDataAsync(SensorData sensorData)
        {
            return database.DeleteAsync(sensorData);
        }

        // Extra 1 - Obter dados dos sensores por UserId
        public Task<List<SensorData>> GetSensorDataByUserIdAsync(int userId)
        {
            return database.Table<SensorData>().Where(s => s.UserId == userId).ToListAsync();
        }

        // Extra 2 - Obter dados dos sensores com nível de estresse acima de um valor
        public Task<List<SensorData>> GetSensorDataByStressLevelAsync(int minStressLevel)
        {
            return database.Table<SensorData>().Where(s => s.StressLevel >= minStressLevel).ToListAsync();
        }

        // Extra 3 - Obter os últimos dados dos sensores (limit 10)
        public Task<List<SensorData>> GetLastSensorDataAsync(int limit = 10)
        {
            return database.Table<SensorData>().OrderByDescending(s => s.Timestamp).Take(limit).ToListAsync();
        }

        // Extra 4 - Contar o número de registros de dados dos sensores
        public Task<int> GetSensorDataCountAsync()
        {
            return database.Table<SensorData>().CountAsync();
        }

        // Extra 5 - Obter dados dos sensores em um intervalo de tempo
        public Task<List<SensorData>> GetSensorDataByTimeRangeAsync(DateTime startTime, DateTime endTime)
        {
            return database.Table<SensorData>()
                           .Where(s => s.Timestamp >= startTime && s.Timestamp <= endTime)
                           .ToListAsync();
        }
    }
}