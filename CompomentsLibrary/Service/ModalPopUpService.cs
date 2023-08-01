namespace CompomentsLibrary.Service
{
    public class ModalPopUpService
    {
        public event Func<string, string, string, string, Task<bool>>? OnShow;

        public async Task<bool> ShowPopup(string headerText = "", string bodyText = "", string yesText = "Ok", string noText = "Cancel")
        {
            return await OnShow?.Invoke(headerText, bodyText, yesText, noText);
        }
    }
}
