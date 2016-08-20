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
            writer.WriteRawValue(((TimeSpan) value).TotalSeconds.ToString("n0"));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return TimeSpan.FromSeconds((double) reader.Value);
        }
    }
}