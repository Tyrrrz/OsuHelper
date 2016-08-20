// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <SecondsToTimespanConverter.cs>
//  Created By: Alexey Golub
//  Date: 20/08/2016
// ------------------------------------------------------------------ 

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OsuHelper.Models.Converters
{
    public class SecondsToTimespanConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((TimeSpan) value).TotalSeconds + "");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            if (reader.Value is double)
                return TimeSpan.FromSeconds((double) reader.Value);
            if (reader.Value is long)
                return TimeSpan.FromSeconds((long) reader.Value);
            string asString = reader.Value as string;
            if (asString != null)
                return TimeSpan.FromSeconds(double.Parse((string) reader.Value));
            return null;
        }
    }
}