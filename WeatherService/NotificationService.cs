using Microsoft.Toolkit.Uwp.Notifications;

namespace WeatherService
{
    public static class NotificationService
    {
        public static void ShowNotification(WeatherResponse currentWeather, List<string> warnings)
        {
            var toastBuilder = new ToastContentBuilder()
                .AddArgument("action", "viewWeather")
                .AddArgument("conversationId", 12345)
                .AddText($"{currentWeather.Weather.First().Main} Temp: {currentWeather.Main.Temp}°C  Wiatr: {currentWeather.Wind.Speed}m/s");

            if(warnings.Count > 0)
            {
                var conWarnings = string.Join(" ", warnings);
                toastBuilder.AddText("Ostrzeżenie: " + conWarnings);
            }
            
            toastBuilder.Show();
        }
    }
}
