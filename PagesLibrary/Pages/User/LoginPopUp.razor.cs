using Microsoft.AspNetCore.Components;

namespace PagesLibrary.Pages.User
{
    public partial class LoginPopUp
    {
        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }

        [Inject] Service.LoginPopUpService loginPopUpService { get; set; }

        protected override void OnInitialized()
        {
            loginPopUpService.OnShow += DisplayLogin;
            loginPopUpService.OnHide += Close;
        }

        public void DisplayLogin()
        {
            IsVisible = true;
            StateHasChanged();
        }

        private void Close()
        {
            IsVisible = false;
            StateHasChanged();
        }

        private bool IsInside { get; set; }
        private void MouseClose()
        {
            if (!IsInside)
            {
                Close();
            }
        }
        private void MouseOut()
        {
            IsInside = false;
        }
        private void MouseEnter()
        {
            IsInside = true;
        }
    }

}
