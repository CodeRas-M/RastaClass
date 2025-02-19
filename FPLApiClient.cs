using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RastaClass
{
    public class FPLApiClient
    {
        private readonly HttpClient _httpClient;
        private Dictionary<int, string> _teamNames;

        public FPLApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://fantasy.premierleague.com/api/");
            _teamNames = new Dictionary<int, string>();
        }

        public async Task<string> GetDataAsync(string endpoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<Fixture>> GetFixturesAsync()
        {
            string json = await GetDataAsync("fixtures/");
            return JArray.Parse(json).ToObject<List<Fixture>>();
        }

        public async Task<Dictionary<int, string>> GetTeamNamesAsync()
        {
            if (_teamNames.Count > 0)
                return _teamNames;

            string json = await GetDataAsync("bootstrap-static/");
            JObject data = JObject.Parse(json);
            var teams = data["teams"].ToObject<List<Team>>();

            foreach (var team in teams)
            {
                _teamNames[team.Id] = team.Name;
            }

            return _teamNames;
        }
    }

    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}