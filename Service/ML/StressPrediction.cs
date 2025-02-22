using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pap.Service.ML;
// 🔹 Classe para os dados de saída (Previsão)
public class StressPrediction
{
    [ColumnName("Score")]
    public float StressLevel { get; set; }
}