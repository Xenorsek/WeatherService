using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService
{
    public class WeatherService
    {
        //&lat=53.8954&lon=20.3165
        private readonly HttpClient _client;
        private const string ApiKey = "apikey";
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5";
        private const string unit = "metric";
        private const string cityName = "Olsztyn, PL";
        private const string cityId = "763166";

        public WeatherService()
        {
            _client = new HttpClient();
        }

        public async Task<WeatherResponse> GetCurrentWeatherResponse()
        {
            var url = $"{BaseUrl}/weather?appid={ApiKey}&units={unit}&id={cityId}";
            var response = await _client.GetAsync(url);
            Console.WriteLine("Dane z OpenWeatherMap CurrentWeather zostały pobrane");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var weatherData = JsonConvert.DeserializeObject<WeatherResponse>(content);

            return weatherData;
        }

        public async Task<ForecastResponse> GetForecastResponse()
        {
            var url = $"{BaseUrl}/forecast?appid={ApiKey}&units={unit}&id={cityId}";
            var response = await _client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Dane z OpenWeatherMap Forecast zostały pobrane");
            var forecastData = JsonConvert.DeserializeObject<ForecastResponse>(content);
            return forecastData;
        }
    }

    public class ForecastResponse
    {
        public List<WeatherResponse> List { get; set; }
    }

    public class WeatherResponse
    {
        // Dopasuj do struktury JSON odpowiedzi
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public List<Weather> Weather { get; set; }
        public Clouds Clouds { get; set; }
        public DateTime Dt_txt { get; set; }
        public double Pop { get; set; }
        public Rain Rain { get; set; }

    }

    public class Rain
    {
        [JsonProperty("3h")]
        public double VolumeLast3Hours { get; set; }
    }
    public class Clouds
    {
        public int All { get; set; }
    }

    public class Weather
    {
        public string Main { get; set; }
        public string Description { get; set; }

    }

    public class Main
    {
        public double Temp { get; set; }
        public double Feels_like { get; set; }
        public double Temp_min { get; set; }
        public double Temp_max { get; set; }
        public int Humidity { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
        public double Gust { get; set; }
    }
}
