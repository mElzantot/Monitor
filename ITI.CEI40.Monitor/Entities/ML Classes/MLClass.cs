using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITI.CEI40.Monitor.Entities
{
    public static class MLClass
    {
        public static PredictedDuration PredictDurationBasedonQualityandCompl(IEnumerable<SubTask> trainSet , SubTask SubTaskSample)
        {
            // ------------Create ML instance  "Similar To DB Context"
            var ctx = new MLContext();

            //------Read My Data  for Training and Testing
            var trainData = ctx.Data.LoadFromEnumerable<SubTask>(trainSet);
            var testTrainSplit = ctx.Data.TrainTestSplit(trainData, testFraction: 0.2);


            //----------Build Model PipeLine

            var dataProcessPipeline = ctx.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(SubTask.ActualDuration))
                            .Append(ctx.Transforms.Categorical.OneHotEncoding(outputColumnName: "FK_EngineerID", inputColumnName: nameof(SubTask.FK_EngineerID)))
                            .Append(ctx.Transforms.NormalizeMinMax(outputColumnName: nameof(SubTask.Complexity)))
                            .Append(ctx.Transforms.NormalizeMinMax(outputColumnName: nameof(SubTask.Quality)))
                            .Append(ctx.Transforms.Concatenate("Features", "FK_EngineerID", nameof(SubTask.Complexity), nameof(SubTask.Quality)))
                            .Append(ctx.Regression.Trainers.Sdca("Label","Features"));

            //----------Train The Model

            var trainedModel = dataProcessPipeline.Fit(testTrainSplit.TrainSet);

            //--------Evaluate Model

            var prediction = trainedModel.Transform(testTrainSplit.TestSet);
            var metrics = ctx.Regression.Evaluate(prediction,"Label","Score");

            //-------Consume Model
            // Create prediction engine related to the loaded trained model
            var predEngine = ctx.Model.CreatePredictionEngine<SubTask, PredictedDuration>(trainedModel);

            //--------Score
            var resultprediction = predEngine.Predict(SubTaskSample);

            PredictedDuration predictedDuration = new PredictedDuration
            {
                Duration = resultprediction.Duration,
                Rsquared = metrics.RSquared
            };


            return predictedDuration;

        }



    }
}
