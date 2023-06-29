using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PagesLibrary.Pages.User
{
    public partial class RegisterPage
    {
        RegisterUser _user = new RegisterUser();
        EditContext? _editContext;
        private bool? _formInvalid = false;
        string? error = "";
        bool showPassword = false;

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
                    var result = await _logInOut.LogIn(_user.Email, _user.Password, false);
                    if (result.Item2 == System.Net.HttpStatusCode.OK)
                    {
                        _user = new RegisterUser();
                        await _authStateProvider.GetAuthenticationStateAsync();
                    }
                    else if (result.Item2 != System.Net.HttpStatusCode.OK)
                    {
                        _ilogger.LogError(result.Item1);
                        _user.Password = "";
                    }
                    if (result.Item2 == System.Net.HttpStatusCode.NoContent)
                    {
                        error = result.Item1;
                    }
                    if (result.Item2 == System.Net.HttpStatusCode.NotFound)
                    {
                        error = result.Item1;
                    }
                    if (result.Item2 == System.Net.HttpStatusCode.BadRequest)
                    {
                        error = "Wystąpił inny błąd";
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
