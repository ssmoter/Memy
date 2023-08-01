using CompomentsLibrary.Helper;

namespace CompomentsLibrary.Service
{
    public class PopupListService
    {
        public event Action<string, string, PopupLevel.Level, int>? OnShow;

        public void ShowToats(string bodyText, string headerText = "", PopupLevel.Level level = PopupLevel.Level.None, int time = 5)
        {
            OnShow?.Invoke(bodyText, headerText, level, time);
        }
    }
}
