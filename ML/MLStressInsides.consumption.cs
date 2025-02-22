﻿// This file was auto-generated by ML.NET Model Builder.
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace StressWatchML
{
    public partial class MLStressInsides
    {
        /// <summary>
        /// model input class for MLStressInsides.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [LoadColumn(0)]
            [ColumnName(@"BPM")]
            public float BPM { get; set; }

            [LoadColumn(1)]
            [ColumnName(@"SpO2")]
            public float SpO2 { get; set; }

            [LoadColumn(2)]
            [ColumnName(@"GSR")]
            public float GSR { get; set; }

            [LoadColumn(3)]
            [ColumnName(@"Temp")]
            public float Temp { get; set; }

            [LoadColumn(4)]
            [ColumnName(@"Nível de Estresse")]
            public float Nível_de_Estresse { get; set; }

            [LoadColumn(5)]
            [ColumnName(@"Conselho")]
            public string Conselho { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for MLStressInsides.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName(@"BPM")]
            public float BPM { get; set; }

            [ColumnName(@"SpO2")]
            public float SpO2 { get; set; }

            [ColumnName(@"GSR")]
            public float GSR { get; set; }

            [ColumnName(@"Temp")]
            public float Temp { get; set; }

            [ColumnName(@"Nível de Estresse")]
            public float Nível_de_Estresse { get; set; }

            [ColumnName(@"Conselho")]
            public uint Conselho { get; set; }

            [ColumnName(@"Features")]
            public float[] Features { get; set; }

            [ColumnName(@"PredictedLabel")]
            public string PredictedLabel { get; set; }

            [ColumnName(@"Score")]
            public float[] Score { get; set; }

        }

        #endregion


        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine().Result, true);


        private static async Task<PredictionEngine<ModelInput, ModelOutput>> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            using var stream = await FileSystem.OpenAppPackageFileAsync("MLStressInsides.mlnet");
            ITransformer mlModel = mlContext.Model.Load(stream, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }

        /// <summary>
        /// Use this method to predict scores for all possible labels.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static IOrderedEnumerable<KeyValuePair<string, float>> PredictAllLabels(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            var result = predEngine.Predict(input);
            return GetSortedScoresWithLabels(result);
        }

        /// <summary>
        /// Map the unlabeled result score array to the predicted label names.
        /// </summary>
        /// <param name="result">Prediction to get the labeled scores from.</param>
        /// <returns>Ordered list of label and score.</returns>
        /// <exception cref="Exception"></exception>
        public static IOrderedEnumerable<KeyValuePair<string, float>> GetSortedScoresWithLabels(ModelOutput result)
        {
            var unlabeledScores = result.Score;
            var labelNames = GetLabels(result);

            Dictionary<string, float> labledScores = new Dictionary<string, float>();
            for (int i = 0; i < labelNames.Count(); i++)
            {
                // Map the names to the predicted result score array
                var labelName = labelNames.ElementAt(i);
                labledScores.Add(labelName.ToString(), unlabeledScores[i]);
            }

            return labledScores.OrderByDescending(c => c.Value);
        }

        /// <summary>
        /// Get the ordered label names.
        /// </summary>
        /// <param name="result">Predicted result to get the labels from.</param>
        /// <returns>List of labels.</returns>
        /// <exception cref="Exception"></exception>
        private static IEnumerable<string> GetLabels(ModelOutput result)
        {
            var schema = PredictEngine.Value.OutputSchema;

            var labelColumn = schema.GetColumnOrNull("Conselho");
            if (labelColumn == null)
            {
                throw new Exception("Conselho column not found. Make sure the name searched for matches the name in the schema.");
            }

            // Key values contains an ordered array of the possible labels. This allows us to map the results to the correct label value.
            var keyNames = new VBuffer<ReadOnlyMemory<char>>();
            labelColumn.Value.GetKeyValues(ref keyNames);
            return keyNames.DenseValues().Select(x => x.ToString());
        }

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }
    }
}
