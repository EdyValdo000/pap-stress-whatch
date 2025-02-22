using SQLite;
namespace pap.Model;

public class SensorData
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; } // Identificador único do registro
    public int UserId { get; set; } // Identificador do usuário associado aos dados
    public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Data e hora da leitura
    public float GSR { get; set; } // Condutividade da pele (Ohms ou kΩ)
    public float BPM { get; set; } // Frequência cardíaca (batimentos por minuto)
    public float SpO2 { get; set; } // Saturação de oxigênio no sangue (%)
    public float Temperature { get; set; } // Temperatura corporal (°C)
    public float StressLevel { get; set; } // Nível de estresse calculado (0 a 100)
    public string Advice { get; set; } = string.Empty; // Conselho gerado com base nos dados
}