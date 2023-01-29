using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCBAWebApp.Converters;

public class DateTimeFormatConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString()!, "dd/MM/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
