using System.Collections.Generic;

namespace RastaClass
{
    public class MatchData
    {
        public float TeamHForm { get; set; } // Home team form
        public float TeamAForm { get; set; } // Away team form
        public float TeamHGoalsScored { get; set; } // Home team goals scored
        public float TeamAGoalsConceded { get; set; } // Away team goals conceded
        public float TeamHHomeAdvantage { get; set; } // Home advantage factor
        public float TeamAAwayDisadvantage { get; set; } // Away disadvantage factor
        public float TeamHStrength { get; set; } // Home team strength
        public float TeamAStrength { get; set; } // Away team strength
        public float PlayerHForm { get; set; } // Home team key player form
        public float PlayerAForm { get; set; } // Away team key player form
        public float PredictedHomeGoals { get; set; } // Label: Actual home goals
        public float PredictedAwayGoals { get; set; } // Label: Actual away goals
       
        private List<MatchData> PrepareMatchData(List<Fixture> fixtures)
        {
            var matchData = new List<MatchData>();

            foreach (var fixture in fixtures)
            {
                var match = new MatchData
                {
                    TeamHForm =(fixture.TeamH),
                    TeamAForm =(fixture.TeamA),
                    TeamHGoalsScored = (fixture.TeamH),
                    TeamAGoalsConceded = (fixture.TeamA),
                    TeamHHomeAdvantage = 1.0f, // Example value
                    TeamAAwayDisadvantage = 0.5f, // Example value
                    TeamHStrength = (fixture.TeamH),
                    TeamAStrength = (fixture.TeamA),
                    PlayerHForm = (fixture.TeamH),
                    PlayerAForm = (fixture.TeamA),
                    PredictedHomeGoals = fixture.TeamHScore ?? 0, // Use actual goals if available
                    PredictedAwayGoals = fixture.TeamAScore ?? 0 // Use actual goals if available
                };

                // Populate additional properties
                fixture.TeamHForm = match.TeamHForm;
                fixture.TeamAForm = match.TeamAForm;
                fixture.TeamHStrength = match.TeamHStrength;
                fixture.TeamAStrength = match.TeamAStrength;

                matchData.Add(match);
            }

            return matchData;
        
        }
    }
}
