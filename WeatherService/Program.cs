using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WeatherService;
Console.WriteLine("Hello, World!");

try
{
    //Pobierz dane
    var weatherDataStorage = new WeatherDataStorage();
    await weatherDataStorage.AddOrUpdateWeatherDataAsync();

    //Analizuj dane
    var weatherAnalyzer = new WeatherAnalyzer();
    var currentWeather = await weatherAnalyzer.CurrentWeatherResponse();
    var warnings = await weatherAnalyzer.AnalyzeWeatherDataAsync();

    //Poinformuj użytkownika

    NotificationService.ShowNotification(currentWeather, warnings);

}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
}