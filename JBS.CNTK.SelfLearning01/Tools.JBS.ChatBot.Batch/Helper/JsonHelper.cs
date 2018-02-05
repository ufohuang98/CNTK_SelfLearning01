using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JBS.ChatBot.Batch.Helper
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerSettings DefaultSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Newtonsoft.Json.Formatting.Indented,
            Converters = new List<JsonConverter> { new StringEnumConverter() { CamelCaseText = true, } },
        };

        public static string ToJson<T>(T obj, JsonSerializerSettings settings = null)
        {
            string json = JsonConvert.SerializeObject(obj, settings ?? DefaultSetting);
            return json.Replace("\r\n", "\n");
        }

        public static T FromJson<T>(string json, JsonSerializerSettings settings = null) where T : class
        {
            return JsonConvert.DeserializeObject<T>(json, settings ?? DefaultSetting);
        }
    }
}
