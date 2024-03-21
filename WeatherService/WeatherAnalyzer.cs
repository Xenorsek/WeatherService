using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace WeatherService
{
    public class WeatherAnalyzer
    {
        private readonly double _dangerTemperature = 0;
        private readonly double _dangerWindSpeed = 10;

        public WeatherAnalyzer()
        {

        }

        public async Task<WeatherResponse> CurrentWeatherResponse()
        {
            var forecastData = await PopulateData();
            var now = DateTime.Now;
            return forecastData.List.Where(x => x.Dt_txt >= now).OrderBy(x => x.Dt_txt).FirstOrDefault();
        }

        private async Task<ForecastResponse> PopulateData()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WeatherApplication","weatherData.json");

            if (!File.Exists(filePath))
            {
                throw new Exception("Plik z danymi pogodowymi nie istnieje.");
            }

            string jsonData = await File.ReadAllTextAsync(filePath);
            var forecastData = JsonConvert.DeserializeObject<ForecastResponse>(jsonData);

            if (forecastData?.List == null)
            {
                throw new Exception("Brak danych w pliku.");
            }
            return forecastData;
        }
        public async Task<List<string>> AnalyzeWeatherDataAsync()
        {

            var forecastData = await PopulateData();

            var warnings = new List<string>();
            var now = DateTime.Now;
            var endOfNextDay = now.AddDays(1).AddHours(12);

            
            foreach (var forecast in forecastData.List.Where(x => x.Dt_txt <= endOfNextDay))
            {

                var date = forecast.Dt_txt;
                var timeLabel = GetTimeOfDayLabel(date, now);
                if (forecast.Main.Temp_min < _dangerTemperature)
                {
                    warnings.Add($"{timeLabel} Przymrozek {forecast.Main.Temp_min}°C.");
                }
                if(forecast.Wind.Speed > _dangerWindSpeed)
                {
                    warnings.Add($"{timeLabel} Silny wiatr prędkość: {forecast.Wind.Speed}m/s.");
                }
            }
            return warnings;
        }
        private string GetTimeOfDayLabel(DateTime forecastTime, DateTime currentTime)
        {
            if (forecastTime.Date > currentTime.Date)
            {
                if (forecastTime.Hour < 12) return "Jutro rano";
                else return "Jutro";
            }
            else
            {
                if (forecastTime.Hour < 6) return "W nocy";
                else if (forecastTime.Hour < 12) return "Rano";
                else if (forecastTime.Hour < 18) return "Po południu";
                else return "Wieczorem";
            }
        }
    }
}
