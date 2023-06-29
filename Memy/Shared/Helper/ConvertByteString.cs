using System.Text;

namespace Memy.Shared.Helper
{
    public static class ConvertByteString
    {
        private static string _salt = "fgsdfg";

        public static string ConvertToString<T>(T value)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, JsonSettings.JsonSerializerSettings());
            var bytes = Encoding.UTF8.GetBytes(json);
            var jsonBytes = Convert.ToBase64String(bytes);
            return _salt + jsonBytes;
        }
        public static T ConvertToObject<T>(string value)
        {
            var byteArr = Convert.FromBase64String(value.Remove(0, _salt.Length));
            string str = Encoding.ASCII.GetString(byteArr);
            var objectFromJson = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            return objectFromJson;
        }
    }
}
