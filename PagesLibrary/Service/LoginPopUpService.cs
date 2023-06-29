namespace PagesLibrary.Service
{
    public class LoginPopUpService
    {
        public event Action OnShow;
        public event Action OnHide;
        public void HidePopUp()
        {
            OnHide?.Invoke();
        }
        public void ShowPopUp()
        {
            OnShow?.Invoke();
        }
    }
}
