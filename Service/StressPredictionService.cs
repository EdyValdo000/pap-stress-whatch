using System;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace StressWhatsh.Services
{
    public class StressPredictionService
    {
        private readonly MLContext mlContext;
        private readonly ITransformer model;
        private readonly PredictionEngine<SensorData, StressPrediction> predictionEngine;

        public StressPredictionService()
        {
            mlContext = new MLContext();
            model = mlContext.Model.Load("stress_model.zip", out _);
            predictionEngine = mlContext.Model.CreatePredictionEngine<SensorData, StressPrediction>(model);
        }

        // Método para prever o nível de estresse com base nos dados do sensor
        public float PredictStressLevel(float bpm, float spo2, float gsr, float temp)
        {
            var inputData = new SensorData { BPM = bpm, SpO2 = spo2, GSR = gsr, Temp = temp };
            var prediction = predictionEngine.Predict(inputData);
            return prediction.StressLevel;
        }
    }

    // 🔹 Classe para os dados de entrada (Sensores)
    public class SensorData
    {
        [LoadColumn(0)] public float BPM { get; set; }
        [LoadColumn(1)] public float SpO2 { get; set; }
        [LoadColumn(2)] public float GSR { get; set; }
        [LoadColumn(3)] public float Temp { get; set; }
        [LoadColumn(4)] public float StressLevel { get; set; }
    }

    // 🔹 Classe para os dados de saída (Previsão)
    public class StressPrediction
    {
        [ColumnName("Score")]
        public float StressLevel { get; set; }
    }
}
