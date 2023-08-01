using CompomentsLibrary.Helper;
using CompomentsLibrary.Model;

namespace CompomentsLibrary.Service
{
    public class ModalAdminMessageService
    {
        public event Func<string, string, PopupLevel.Level, string, string, string, Task<AdminModalResult?>>? OnShow;

        public async Task<AdminModalResult?>? ShowPopup(string headerText = "",
                                     string bodyText = "",
                                     PopupLevel.Level level1 = PopupLevel.Level.None,
                                     string level2 = "none",
                                     string yesText = "Ok",
                                     string noText = "Cancel")
        {
            return await OnShow?.Invoke(headerText, bodyText, level1, level2, yesText, noText);
        }
    }
}
