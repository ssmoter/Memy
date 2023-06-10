namespace PagesLibrary.Data
{
    public static class CompareDate
    {

        public static string GetDate(DateTimeOffset dateTimeOffset)
        {
            var date = DateTimeOffset.Now - dateTimeOffset.AddHours(-2).ToUniversalTime();
            if (date.TotalSeconds < 60)
            {
                return $"{(int)date.TotalSeconds} sekund temu";
            }
            if (date.TotalMinutes < 60)
            {
                return $"{(int)date.TotalMinutes} minut temu";
            }
            if (date.TotalHours < 24)
            {
                return $"{(int)date.TotalHours} godzin temu";
            }
            if (date.TotalDays < 30)
            {
                return $"{(int)date.TotalDays} dni temu";
            }
            return date.ToString("dd.MM.yyyy");
        }
    }
}
