using System.Text;

namespace Memy.Shared.Helper
{
    public static class ConvertByteString
    {
        private static readonly string _saltS = "alcseIsSFx";
        private static readonly string _saltE = "YLEh8WbxJki";
        private static char _equalsSing = '=';

        public static string ConvertToString<T>(T? value)
        {
            ArgumentNullException.ThrowIfNull(value);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, JsonSettings.JsonSerializerSettings());
            var bytes = Encoding.UTF8.GetBytes(json);
            var jsonBytes = Convert.ToBase64String(bytes);
            var result = _saltS + RemoveEquals(jsonBytes).ToString() + _saltE;
            return result;
        }
        public static T? ConvertToObject<T>(string? value)
        {
            ArgumentException.ThrowIfNullOrEmpty(value);
            var onlyValue = GetValue(value).ToString() + _equalsSing;
            var byteArr = Convert.FromBase64String(onlyValue);
            string str = Encoding.ASCII.GetString(byteArr);
            var objectFromJson = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            return objectFromJson;
        }

        private static ReadOnlySpan<Char> GetValue(ReadOnlySpan<char> value)
        {
            var onlyValue = value.Slice(_saltS.Length, value.Length - (_saltE.Length + _saltS.Length));
            return onlyValue;
        }

        private static ReadOnlySpan<Char> RemoveEquals(ReadOnlySpan<char> value)
        {
            return value.Slice(0, value.Length - 1);
        }
    }
}
