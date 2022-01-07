using Microsoft.ML;
using Microsoft.ML.Trainers;
using MongoDB.Driver;
using Movies.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Core
{
    public class MoviesService
    {
        private static string trainingDataPath = Path.Combine(Environment.CurrentDirectory, @"Data\recommendation-ratings-train.csv");
        private static string testDataPath = Path.Combine(Environment.CurrentDirectory, @"Data\recommendation-ratings-test.csv");
        private static string moviesPath = Path.Combine(Environment.CurrentDirectory, @"Data\movie.csv");
        private static List<MovieModel> All = new List<MovieModel>();

        static void Main(string[] args)
        {
            StreamReader reader = new StreamReader(File.OpenRead(moviesPath));

            reader.ReadLine();
            int counter = 2;

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                var nesta = new MovieModel
                {
                    Id = Int32.Parse(values[0]),
                    Title = values[1]
                };

                All.Add(nesta);
            }
            var mlContext = new MLContext();


            var trainingDataView = mlContext.Data.LoadFromTextFile<MovieRatingModel>(trainingDataPath, hasHeader: true, separatorChar: ',');
            var testDataView = mlContext.Data.LoadFromTextFile<MovieRatingModel>(testDataPath, hasHeader: true, separatorChar: ',');

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 100000,
                ApproximationRank = 512,
                Lambda = 0.81,
                LearningRate = 0.000196
            };

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(
                    inputColumnName: "userId",
                    outputColumnName: "userIdEncoded")
                .Append(mlContext.Transforms.Conversion.MapValueToKey(
                    inputColumnName: "movieId",
                    outputColumnName: "movieIdEncoded"));


            var trainingDataEstimator = pipeline.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));

            Console.WriteLine("Training the model...");
            var model = trainingDataEstimator.Fit(trainingDataView);
            Console.WriteLine();

            Console.WriteLine("=============== Evaluating the model ===============");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");

            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
            Console.WriteLine("RSquared: " + metrics.LossFunction.ToString());
            Console.WriteLine("RSquared: " + metrics.MeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.MeanAbsoluteError.ToString());
            Console.WriteLine();

            var predictionEngine = mlContext.Model.CreatePredictionEngine<MovieRatingModel, MovieRatingPredictionModel>(model);
            Console.WriteLine("Calculating the top 5 movies for user 611...");
            var top5 = (from m in All
                        let p = predictionEngine.Predict(
                           new MovieRatingModel()
                           {
                               userId = 611,
                               movieId = m.Id
                           })
                        orderby p.Score descending
                        select (MovieId: m.Id, Score: p.Score)).Take(5);

            foreach (var t in top5)
                Console.WriteLine($"  Score:{t.Score}\tMovie: {Dohvati(t.MovieId)?.Title}");



        }

        public static MovieModel Dohvati(int id)
        {
            return All.Single(m => m.Id == id);
        }
    }
}

