using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages.User.Register
{
    public partial class RegisterPage
    {

        protected override void OnInitialized()
        {
            _ilogger.LogInformation("Initialized page");
            _editContext = new EditContext(_user);
            _editContext.OnFieldChanged += HandleFieldChanged;
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
                    ArgumentNullException.ThrowIfNull(_user);
                    ArgumentNullException.ThrowIfNullOrEmpty(_user.Nick);
                    ArgumentNullException.ThrowIfNullOrEmpty(_user.Email);
                    ArgumentNullException.ThrowIfNullOrEmpty(_user.Password);

                    var result = await _registerApi.PostRegister(_user.Nick, _user.Email, _user.Password);
                    var json = await result.Content.ReadAsStringAsync();
                    if (result.IsSuccessStatusCode)
                    {
                        _registerPass = true;
                        _email = _user.Email;
                        _user = new RegisterUser();
                    }
                    else
                    {
                        _ilogger.LogError(json);
                        _user.Password = "";
                        if (json == "email")
                        {
                            _error = "Dany email został już wykorzystany";
                        }
                        if (json == "name")
                        {
                            _error = "Dana nazwa użytkownika już istnieje";
                        }
                    }
                }
                catch (Exception ex)
                {
                    _ilogger.LogError(ex.Message);
                }
            }
        }

        public void Dispose()
        {
            _ilogger.LogInformation("Dispose");
            if (_editContext != null)
            {
                _editContext.OnFieldChanged -= HandleFieldChanged;
            }
            _user = new RegisterUser();
        }

        private class RegisterUser
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
            public string? Nick { get; set; }
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email jest wymagany")]
            [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Email jest nieprawidłowy")]
            public string? Email { get; set; }
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Hasło jest wymagane")]
            [System.ComponentModel.DataAnnotations.StringLength(1000, MinimumLength = 8, ErrorMessage = "Hasło jest za krótkie")]
            public string? Password { get; set; }
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Hasło jest wymagane")]
            [System.ComponentModel.DataAnnotations.StringLength(1000, MinimumLength = 8, ErrorMessage = "Hasło jest za krótkie")]
            [System.ComponentModel.DataAnnotations.Compare(nameof(Password), ErrorMessage = "Hasła się różnią")]
            public string? PasswordConfirm { get; set; }
            [System.ComponentModel.DataAnnotations.Compare(nameof(TrueBool), ErrorMessage = "Wymagane jest zatwierdzenie regulaminu")]
            public bool Statute { get; set; }
            public bool TrueBool { get => true; }

        }

    }
}
