using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using WhojooSite.Common.Web;

namespace WhojooSite.Recipes.Module.Weather;

public static class GetWeatherForecast
{
    public class Endpoint : IEndpoint
    {
        public void AddEndpoint(IEndpointRouteBuilder app)
        {
            app
                .MapGet("/weatherforecast", Handle)
                .WithName("GetWeatherForecast")
                .WithOpenApi();
        }

        public static IResult Handle()
        {
            string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return TypedResults.Ok(forecast);
        }


        record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
        {
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }
    }
}