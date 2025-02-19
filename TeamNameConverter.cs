using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace RastaClass
{
    public class TeamNameConverter : IValueConverter
    {
        private static Dictionary<int, string> _teamNames;

        public TeamNameConverter()
        {
            // Initialize team names (this will be populated later)
            _teamNames = new Dictionary<int, string>();
        }

        public static void SetTeamNames(Dictionary<int, string> teamNames)
        {
            _teamNames = teamNames;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int teamId && _teamNames.ContainsKey(teamId))
            {
                string teamName = _teamNames[teamId];
                return teamName.Substring(0, Math.Min(3, teamName.Length)).ToUpper();
            }
            return "UNK";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}