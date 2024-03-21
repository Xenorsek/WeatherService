using Newtonsoft.Json;

namespace WeatherService
{
    public class WeatherDataStorage
    {
        private readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WeatherApplication", "weatherData.json");
        public WeatherDataStorage() {

            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public async Task AddOrUpdateWeatherDataAsync()
        {
            ForecastResponse existingData = await ReadExistingDataAsync();

            // Sprawdzenie, czy istnieje wpis dla "dzisiaj"
            var lastEntry = existingData?.List?.OrderByDescending(x => x.Dt_txt).FirstOrDefault();
            var lastDate = lastEntry?.Dt_txt ?? DateTime.MinValue;

            var substrateDate = DateTime.Now.Subtract(lastDate).TotalDays;
            if (substrateDate <= -2)
            {
                // Jeśli różnica między datami jest mniejsza lub równa 1 dniu, aktualizujemy dane
                Console.WriteLine("Dane są aktualne. Nie dodajemy nowych danych.");
                return;
            }

            // Zapis nowych danych do pliku
            using (StreamWriter file = File.CreateText(_filePath))
            {
                var weatherService = new WeatherService();
                var data = await weatherService.GetForecastResponse();
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }

            Console.WriteLine("Dane prognozy pogody zostały zaktualizowane.");
        }

        private async Task<ForecastResponse> ReadExistingDataAsync()
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            using (StreamReader file = File.OpenText(_filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize(file, typeof(ForecastResponse)) as ForecastResponse;
            }
        }
    }
}
