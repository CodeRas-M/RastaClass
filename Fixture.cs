namespace RastaClass
{
    public class Fixture
    {
        public int Id { get; set; }
        public int? Event { get; set; } // Gameweek
        public int TeamH { get; set; } // Home team ID
        public int TeamA { get; set; } // Away team ID
        public int? TeamHScore { get; set; } // Home team score
        public int? TeamAScore { get; set; } // Away team score
        public bool Finished { get; set; }
        public float PredictedHomeGoals { get; set; } // Predicted home goals
        public float PredictedAwayGoals { get; set; } // Predicted away goals
        public string PredictedOutcome { get; set; } // Predicted outcome (W, D, L)
        public string PredictedScore => $"{PredictedHomeGoals:0} - {PredictedAwayGoals:0}"; // Predicted scoreline
        public float TeamHForm { get; set; } // Home team form
        public float TeamAForm { get; set; } // Away team form
        public float TeamHStrength { get; set; } // Home team strength
        public float TeamAStrength { get; set; } // Away team strength
    }
}