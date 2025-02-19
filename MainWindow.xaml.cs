using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;

namespace RastaClass
{
    public partial class MainWindow : Window
    {
        private readonly FPLApiClient _apiClient;
        private readonly PModel _pModel;

        public MainWindow()
        {
            InitializeComponent();
            _apiClient = new FPLApiClient();
            _pModel = new PModel();
            LoadTeamNames();
            LoadPredictions();
        }

        private async void LoadTeamNames()
        {
            try
            {
                // Fetch team names from the FPL API
                var teamNames = await _apiClient.GetTeamNamesAsync();

                // Pass team names to the TeamNameConverter
                TeamNameConverter.SetTeamNames(teamNames);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching team names: {ex.Message}");
            }
        }

        private async void LoadPredictions()
        {
            try
            {
                // Fetch fixtures from the FPL API
                List<Fixture> fixtures = await _apiClient.GetFixturesAsync();

                // Prepare match data for predictions
                List<MatchData> matchData = PrepareMatchData(fixtures);

                // Train the model (if not already trained)
                _pModel.TrainModel(matchData);

                // Make predictions for each fixture
                foreach (var fixture in fixtures)
                {
                    // Skip finished fixtures
                    if (fixture.Finished)
                        continue;

                    var match = new MatchData
                    {
                        TeamHForm = CalculateTeamForm(fixture.TeamH),
                        TeamAForm = CalculateTeamForm(fixture.TeamA),
                        TeamHGoalsScored = CalculateGoalsScored(fixture.TeamH),
                        TeamAGoalsConceded = CalculateGoalsConceded(fixture.TeamA),
                        TeamHHomeAdvantage = 1.0f, // Example value
                        TeamAAwayDisadvantage = 0.5f // Example value
                    };

                    // Predict home and away goals
                    fixture.PredictedHomeGoals = _pModel.PredictHomeGoals(match);
                    fixture.PredictedAwayGoals = _pModel.PredictAwayGoals(match);

                    // Calculate predicted outcome
                    if (fixture.PredictedHomeGoals > fixture.PredictedAwayGoals)
                    {
                        fixture.PredictedOutcome = "W (1)"; // Home team wins
                    }
                    else if (fixture.PredictedHomeGoals < fixture.PredictedAwayGoals)
                    {
                        fixture.PredictedOutcome = "L (-1)"; // Home team loses
                    }
                    else
                    {
                        fixture.PredictedOutcome = "D (0)"; // Draw
                    }

                    // Populate additional properties
                    fixture.TeamHForm = match.TeamHForm;
                    fixture.TeamAForm = match.TeamAForm;
                    fixture.TeamHStrength = match.TeamHStrength;
                    fixture.TeamAStrength = match.TeamAStrength;
                }

                // Display only upcoming fixtures with predicted outcomes
                var upcomingFixtures = fixtures.Where(f => !f.Finished).ToList();
                FixturesDataGrid.ItemsSource = upcomingFixtures;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating predictions: {ex.Message}");
            }
        }

        // Other methods (PrepareMatchData, CalculateTeamForm, etc.) remain unchanged
    



private List<MatchData> PrepareMatchData(List<Fixture> fixtures)
        {
            var matchData = new List<MatchData>();

            foreach (var fixture in fixtures)
            {
                var match = new MatchData
                {
                    TeamHForm = CalculateTeamForm(fixture.TeamH),
                    TeamAForm = CalculateTeamForm(fixture.TeamA),
                    TeamHGoalsScored = CalculateGoalsScored(fixture.TeamH),
                    TeamAGoalsConceded = CalculateGoalsConceded(fixture.TeamA),
                    TeamHHomeAdvantage = 1.0f, // Example value
                    TeamAAwayDisadvantage = 0.5f, // Example value
                    TeamHStrength = GetTeamStrength(fixture.TeamH),
                    TeamAStrength = GetTeamStrength(fixture.TeamA),
                    PlayerHForm = GetPlayerForm(fixture.TeamH),
                    PlayerAForm = GetPlayerForm(fixture.TeamA),
                    PredictedHomeGoals = fixture.TeamHScore ?? 0, // Use actual goals if available
                    PredictedAwayGoals = fixture.TeamAScore ?? 0 // Use actual goals if available
                };

                matchData.Add(match);
            }

            return matchData;
        }

        private float CalculateTeamForm(int teamId)
        {
            // Add logic to calculate team form (e.g., average points in last 5 matches)
            return 1.5f; // Example value
        }

        private float CalculateGoalsScored(int teamId)
        {
            // Add logic to calculate goals scored by the team
            return 2.0f; // Example value
        }

        private float CalculateGoalsConceded(int teamId)
        {
            // Add logic to calculate goals conceded by the team
            return 1.0f; // Example value
        }

        private float GetTeamStrength(int teamId)
        {
            // Add logic to get team strength (e.g., from FPL API)
            return 3.0f; // Example value
        }

        private float GetPlayerForm(int teamId)
        {
            // Add logic to get key player form (e.g., from FPL API)
            return 4.0f; // Example value
        }
    }
}