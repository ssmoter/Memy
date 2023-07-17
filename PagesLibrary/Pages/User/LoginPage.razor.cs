using CompomentsLibrary.Helper;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages.User
{
    public partial class LoginPage
    {
        LoginUser _user = new LoginUser();
        EditContext? _editContext;
        private bool? _formInvalid = false;
        string? error = "";

        protected override void OnInitialized()
        {
            _editContext = new EditContext(_user);
            _editContext.OnFieldChanged += HandleFieldChanged;
#if DEBUG
            _ilogger.LogInformation("Initialized page");
#endif
        }
        public void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            if (_editContext != null)
            {
                _formInvalid = !_editContext.Validate();
            }
            StateHasChanged();
        }

        private async Task HandleValidSubmit()
        {
            _ilogger.LogInformation("Valid Submit");
            if (_editContext != null)
            {
                try
                {
                    var result = await _logInOut.LogIn(_user.Email, _user.Password, _user.DoNotLogOut);
                    if (result.Item2 == System.Net.HttpStatusCode.OK)
                    {
                        _user = new LoginUser();
                        await _authStateProvider.GetAuthenticationStateAsync();
                        _popUp.ShowToats("Zostałeś zalogowany", "Status logowania", PopupLevel.Level.Success);
                        _loginPopUp.HidePopUp();
                    }
                    else if (result.Item2 != System.Net.HttpStatusCode.OK)
                    {
                        _ilogger.LogError(result.Item1);
                        _user.Password = "";
                        _popUp.ShowToats("Błąd logowania", "Status logowania", PopupLevel.Level.Warning);
                    }
                    if (result.Item2 == System.Net.HttpStatusCode.NoContent)
                    {
                        _ilogger.LogError(result.Item1);
                        error = result.Item1;
                    }
                    if (result.Item2 == System.Net.HttpStatusCode.NotFound)
                    {
                        _ilogger.LogError(result.Item1);
                        error = result.Item1;
                    }
                    if (result.Item2 == System.Net.HttpStatusCode.BadRequest)
                    {
                        _ilogger.LogError(result.Item1);
                        error = "Wystąpił inny błąd";
                    }
                }
                catch (Exception ex)
                {
                    error = "Wystąpił inny błąd";
                    _ilogger.LogError(ex.Message);
                }
            }
        }


        public void Dispose()
        {
            if (_editContext != null)
            {
                _editContext.OnFieldChanged -= HandleFieldChanged;
            }
            _editContext = null;
            _user = null;
            error = "";
#if DEBUG
            _ilogger.LogInformation("Dispose");
#endif
        }

        class LoginUser
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email jest wymagany")]
            [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Email jest nieprawidłowy")]
            public string? Email { get; set; }
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Hasło jest wymagane")]
            [System.ComponentModel.DataAnnotations.StringLength(1000, MinimumLength = 8, ErrorMessage = "Hasło jest za krótkie")]
            public string? Password { get; set; }
            public bool DoNotLogOut { get; set; } = false;
        }
    }
}
