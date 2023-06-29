using Newtonsoft.Json;

namespace Memy.Shared.Helper
{
    public static class JsonSettings
    {
        public static JsonSerializerSettings JsonSerializerSettings()
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore,
            };
            return settings;
        }
    }
}
