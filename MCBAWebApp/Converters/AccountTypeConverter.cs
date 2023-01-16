using MCBADataLibrary.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCBAWebApp.Converters;

public class AccountTypeConverter : JsonConverter<AccountType>
{
    public override AccountType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonAccountType = reader.GetString()!;
        if (jsonAccountType == "S")
        {
            return AccountType.Savings;
        }
        else
        {
            return AccountType.Checking;
        }
    }

    public override void Write(Utf8JsonWriter writer, AccountType value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
