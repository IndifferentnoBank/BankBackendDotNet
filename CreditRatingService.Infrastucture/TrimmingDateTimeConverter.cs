using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace CreditRatingService.Infrastucture
{
    public class TrimmingDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string dateString = reader.GetString();
                if (!string.IsNullOrEmpty(dateString) && dateString.Length > 5)
                {
                    dateString = dateString.Substring(0, dateString.Length - 5);
                }

                if (DateTime.TryParse(dateString, out DateTime date))
                {
                    return date;
                }
            }

            throw new JsonException("Invalid date format.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("O"));
        }
    }
}
