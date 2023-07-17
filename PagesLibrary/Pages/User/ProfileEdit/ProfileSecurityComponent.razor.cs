using Memy.Shared.Helper;

using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages.User.ProfileEdit
{
    public partial class ProfileSecurityComponent : IDisposable
    {
        protected override async Task OnInitializedAsync()
        {
            try
            {
                _user.Email = await _profileData.GetEmailAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public void Dispose()
        {
            _editPassword = null;
        }

        private async Task HandleValidSubmitPassword()
        {
            try
            {
                var password = new Memy.Shared.Model.Password()
                {
                    New = _editPassword.Password,
                    Old = _editPassword.OldPassword,
                };

                var strBytes = ConvertByteString.ConvertToString(password);

                var result = await _profileData.UpdateProfil(Memy.Shared.Helper.MyEnums.UpdateProfile.Password, strBytes);
                var json = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    _popUp.ShowToats("Zaktualizowano hasło", "Success", CompomentsLibrary.Helper.PopupLevel.Level.Success);
                    _logger.LogInformation(json);
                    _editPassword = new();

                }
                else
                {
                    _popUp.ShowToats("Nie udało się zaktualizować hasła", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                    _logger.LogWarning(json);

                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Wystąpił błąd", "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }
        private async Task HandleValidSubmitEmail()
        {
            try
            {
                var result = await _profileData.UpdateProfil(Memy.Shared.Helper.MyEnums.UpdateProfile.Email, _user.Email);
                var json = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    _popUp.ShowToats("Zaktualizowano email", "Success", CompomentsLibrary.Helper.PopupLevel.Level.Success);
                    _logger.LogInformation(json);

                }
                else
                {
                    _popUp.ShowToats("Nie udało się zaktualizować email", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                    _logger.LogWarning(json);

                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Wystąpił błąd", "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }



        private class EditPassword
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Hasło jest wymagane")]
            public string? OldPassword { get; set; }
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Hasło jest wymagane")]
            [System.ComponentModel.DataAnnotations.StringLength(1000, MinimumLength = 8, ErrorMessage = "Hasło jest za krótkie")]
            public string? Password { get; set; }
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Hasło jest wymagane")]
            [System.ComponentModel.DataAnnotations.StringLength(1000, MinimumLength = 8, ErrorMessage = "Hasło jest za krótkie")]
            [System.ComponentModel.DataAnnotations.Compare(nameof(Password), ErrorMessage = "Hasła się różnią")]
            public string? PasswordConfirm { get; set; }
        }
        private class EditUser
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email jest wymagany")]
            [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage = "Email jest nieprawidłowy")]
            public string? Email { get; set; }

        }

    }
}
