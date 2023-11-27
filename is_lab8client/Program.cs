//using is_lab8service;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.ServiceModel;
using System.Threading.Tasks;

namespace is_lab8client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GetForecast();
            //await GetForecastByDate(DateTime.Parse("12.12.2023"));
            Console.ReadLine();

            static async Task GetForecast()
            {
                var client = new HttpClient();
                var response = await client.GetAsync("https://localhost:44366/WeatherForecast");
                var forecasts = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();

                foreach (var forecast in forecasts)
                {
                    Console.WriteLine($"Дата: {forecast.Date}, Температура, C: {forecast.TemperatureC}, Погода: {forecast.Summary}");
                }
            }

            static async Task GetForecastByDate(DateTime date)
            {
                var client = new HttpClient();
                var response = await client.GetAsync($"https://localhost:44366/WeatherForecast/{date}");
                var forecasts = await response.Content.ReadFromJsonAsync<List<WeatherForecast>>();

                foreach (var forecast in forecasts)
                {
                    Console.WriteLine($"Дата: {forecast.Date}, Температура, C: {forecast.TemperatureC}, Погода: {forecast.Summary}");
                }
            }
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF { get; set; }
        public string Summary { get; set; }
    }
}
