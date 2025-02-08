using pap.Database;
using pap.Model;
using pap.Repositore;

namespace pap.Service;
public class SensorDataService
{
    private readonly HeartOximeterDataRepository _heartOximeterRepositore;
    private readonly TemperatureDataRepository _temperatureRepositore;
    private readonly GSRDataRepository _gsrRepositore;

    public SensorDataService(
        HeartOximeterDataRepository heartOximeterRepositore,
        TemperatureDataRepository temperatureRepositore,
        GSRDataRepository gsrRepositore)
    {
        _heartOximeterRepositore = heartOximeterRepositore;
        _temperatureRepositore = temperatureRepositore;
        _gsrRepositore = gsrRepositore;
    }

    // Salvar todas as leituras dos sensores em uma única operação
    public async Task SaveAll(HeartOximeterData heartData, TemperatureData tempData, GSRData gsrData)
    {
        await _heartOximeterRepositore.SaveHeartOximeterDataAsync(heartData);
        await _temperatureRepositore.SaveTemperatureDataAsync(tempData);
        await _gsrRepositore.SaveGSRDataAsync(gsrData);
    }

    // Buscar a última leitura dos três sensores para uma sessão específica
    public async Task<(HeartOximeterData, TemperatureData, GSRData)> GetLastReadings(int sessionId)
    {
        var heart = await _heartOximeterRepositore.GetLastReadingBySessionAsync(sessionId);
        var temp = await _temperatureRepositore.GetLastReadingBySessionAsync(sessionId);
        var gsr = await _gsrRepositore.GetLastReadingBySessionAsync(sessionId);
        return (heart, temp, gsr);
    }

    // Deletar todas as leituras dos sensores para uma sessão específica
    public async Task DeleteAllBySession(int sessionId)
    {
        var heartData = await _heartOximeterRepositore.GetDataBySessionIdAsync(sessionId);
        var tempData = await _temperatureRepositore.GetDataBySessionIdAsync(sessionId);
        var gsrData = await _gsrRepositore.GetDataBySessionIdAsync(sessionId);

        foreach (var data in heartData) await _heartOximeterRepositore.DeleteHeartOximeterDataAsync(data);
        foreach (var data in tempData) await _temperatureRepositore.DeleteTemperatureDataAsync(data);
        foreach (var data in gsrData) await _gsrRepositore.DeleteGSRDataAsync(data);
    }
}
