using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace RastaClass
{
    public class PModel
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public PModel()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(List<MatchData> matchData)
        {
            IDataView data = _mlContext.Data.LoadFromEnumerable(matchData);

            var pipeline = _mlContext.Transforms.Concatenate("Features",
                    nameof(MatchData.TeamHForm),
                    nameof(MatchData.TeamAForm),
                    nameof(MatchData.TeamHGoalsScored),
                    nameof(MatchData.TeamAGoalsConceded),
                    nameof(MatchData.TeamHHomeAdvantage),
                    nameof(MatchData.TeamAAwayDisadvantage),
                    nameof(MatchData.TeamHStrength),
                    nameof(MatchData.TeamAStrength),
                    nameof(MatchData.PlayerHForm),
                    nameof(MatchData.PlayerAForm))
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: nameof(MatchData.PredictedHomeGoals)));

            _model = pipeline.Fit(data);
        }

        public float PredictHomeGoals(MatchData match)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MatchData, MatchPrediction>(_model);
            return predictionEngine.Predict(match).PredictedHomeGoals;
        }

        public float PredictAwayGoals(MatchData match)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MatchData, MatchPrediction>(_model);
            return predictionEngine.Predict(match).PredictedAwayGoals;
        }
    }

    public class MatchPrediction
    {
        public float PredictedHomeGoals { get; set; }
        public float PredictedAwayGoals { get; set; }
    }
}