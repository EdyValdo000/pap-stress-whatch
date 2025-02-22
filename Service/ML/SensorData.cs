using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pap.Service.ML;
// 🔹 Classe para os dados de entrada (Sensores)
public class SensorData
{
    [LoadColumn(0)] public float BPM { get; set; }
    [LoadColumn(1)] public float SpO2 { get; set; }
    [LoadColumn(2)] public float GSR { get; set; }
    [LoadColumn(3)] public float Temp { get; set; }
}