using Memy.Shared.Helper;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

using PagesLibrary.Data.File;

namespace PagesLibrary.Pages.User.ProfileEdit
{
    public partial class ProfileEditComponent : IDisposable
    {
        protected override async Task OnInitializedAsync()
        {
            try
            {
                _user.Nick = (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.Name;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task HandleValidSubmitName()
        {
            try
            {                

                var result = await _profileData.UpdateProfil(Memy.Shared.Helper.MyEnums.UpdateProfile.Name, _user.Nick);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    _popUp.ShowToats("Zaktualizowano nazwę użytkownika", "Success", CompomentsLibrary.Helper.PopupLevel.Level.Success);
                    Func?.Invoke(_user.Nick);
                    _logger.LogInformation(json);
                }
                else
                {
                    _popUp.ShowToats("Nie udało się zaktualizować nazwy użytkownika", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                    _logger.LogWarning(json);

                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Wystąpił błąd", "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }
        private async Task HandleValidSubmitAvatar()
        {
            try
            {
                if (_avatar == null)
                {
                    return;
                }
                var result = await _profileData.UpdateProfil(Memy.Shared.Helper.MyEnums.UpdateProfile.Avatar, _avatar);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    _popUp.ShowToats("Zaktualizowano avatar użytkownika", "Success", CompomentsLibrary.Helper.PopupLevel.Level.Success);
                    Func?.Invoke(null);
                    _logger.LogInformation(json);
                }
                else
                {
                    _popUp.ShowToats("Nie udało się zaktualizować avatara użytkownika", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                    _logger.LogWarning(json);
                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Wystąpił błąd", "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }

        private async Task PreviewAvatar(InputFileChangeEventArgs args)
        {
            try
            {
                var file = args.File;
                if (file == null)
                {
                    return;
                }
                if (file.Size > 2 * 1024 * 1024)
                {
                    _popUp.ShowToats("Za duży plik", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                    return;
                }
                var avatar = await file.RequestImageFileAsync("svg", 184, 184);
                using (Stream fileStream = avatar.OpenReadStream(2 * 1024 * 1024))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        _avatar = new();
                        _avatar.ObjName = $"Avatar.{avatar.ContentType}";

                        await fileStream.CopyToAsync(ms);
                        _avatar.Data = new byte[ms.Length];
                        _avatar.Data = ms.ToArray();
                        _avatarstr = $"data:image/{avatar.Name};base64,{Convert.ToBase64String(_avatar.Data)}";
                    }
                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Wystąpił błąd", "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }

        private async Task HandleOnDrop(DragEventArgs args)
        {
            var files = args.DataTransfer.Files;
        }



        public void Dispose()
        {
            _user = null;
        }

        private class EditUser
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
            public string? Nick { get; set; }
        }

    }
}
