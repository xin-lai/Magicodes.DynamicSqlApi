using Swashbuckle.AspNetCore.Annotations;

namespace Web6
{
    [SwaggerSchema("²âÊÔ")]
    public class WeatherForecast
    {
        [SwaggerSchema("ÈÕÆÚÃèÊö")]
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}