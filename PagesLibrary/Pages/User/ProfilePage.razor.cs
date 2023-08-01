using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages.User
{
    public partial class ProfilePage : IDisposable
    {

        protected override async Task OnInitializedAsync()
        {
            await CalculateProfile(Name);
#if DEBUG
            _ilogger.LogInformation("Initialized async page");
#endif
        }

        private async Task CalculateProfile(string? name)
        {
            if (name == null)
            {
                name = Name;
            }
            _userPublicModel = await GetProfil(name);

            if (_userPublicModel is not null)
            {
                decimal like = _userPublicModel.SumTaskLike;
                decimal unlike = _userPublicModel.SumTaskUnLike;
                if (unlike == 0)
                {
                    unlike = 1;
                }
                Name = name;

                _percentLike = Math.Abs(Math.Round((like / unlike), 2));
                if (_userPublicModel.CreatedDate is not null)
                {
                    _createdDate = _userPublicModel.CreatedDate.Value.ToString("dd.MM.yyyy");
                }
            }
            await _authStateProvider.GetAuthenticationStateAsync();
            StateHasChanged();
        }

        #region API


        private async Task<Memy.Shared.Model.UserPublicModel?> GetProfil(string? name)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrEmpty(name);
                var result = await _profile.GetProfileAsync(name);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var userModel = Newtonsoft.Json.JsonConvert.DeserializeObject<UserPublicModel>(json);
                    return userModel;
                }
                if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _ilogger.LogWarning(json);
                    _popUp.ShowToats("Nie znaleziono użytkownika", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning, 5);
                }
                if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _ilogger.LogWarning(json);
                    _popUp.ShowToats("Nie podano nazwy", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning, 5);
                }
                return null;
            }
            catch (Exception ex)
            {
                _ilogger.LogError(ex.Message);
                _popUp.ShowToats("Wystąpił błąd", "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error, 5);
                return null;
            }
        }

        private async Task<TaskModel[]?> GetTasksAsync(string? name, int start = 0, int max = 10, int orderTyp = 0, bool banned = false)
        {
            try
            {
                var result = await _fileManager.GetUserTaskModelsAsync(name, start, max, orderTyp, banned);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var task = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskModel[]>(json);
                    return task;
                }
                else
                {
                    _ilogger.LogWarning(json);
                    _popUp.ShowToats("Nie udało się pobrać danych", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                }

                return null;
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Nie udało się pobrać danych", "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _ilogger.LogError(ex.Message);
                return null;
            }
        }

        private async Task<CommentModel[]?> GetCommentAsync(string? name, int? orderTyp)
        {
            try
            {
                var result = await _comment.GetUserCommentAsync(name, orderTyp);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    var CommentModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommentModel[]>(json);
                    return CommentModel;
                }
                else
                {
                    _ilogger.LogWarning(json);
                    _popUp.ShowToats("Nie udało się pobrać danych", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                }

                return null;
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Nie udało się pobrać danych", "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _ilogger.LogError(ex.Message);
                return null;
            }
        }

        #endregion

        #region Button
        private async Task BtnGetTasksAsync()
        {
            _likeSelectedTask = false;
            if (_commentModel != null)
            {
                _commentModel = null;
            }
            var result = await _authStateProvider.GetAuthenticationStateAsync();
            bool banned = false;

            if (result.User.Identity != null)
            {
                if (Name == result.User.Identity.Name)
                {
                    banned = true;
                }
            }

            _taskModels = await GetTasksAsync(Name, _start, _max, int.Parse(_orderTyp.Item1), banned);


            StateHasChanged();
        }
        private async Task BtnGetLikeTasksAsync()
        {
            _likeSelectedTask = true;
            if (_commentModel != null)
            {
                _commentModel = null;
            }

            _taskModels = await GetTasksAsync(null, _start, _max, int.Parse(_orderTyp.Item1));

            StateHasChanged();
        }
        private async Task ChangePage(int index)
        {
            _start = index;
            if (_likeSelectedTask)
                await BtnGetLikeTasksAsync();
            else
                await BtnGetTasksAsync();
        }
        private async Task ChangePageTask()
        {
            _start = 0;
            if (_likeSelectedTask)
                await BtnGetLikeTasksAsync();
            else
                await BtnGetTasksAsync();

        }

        private async Task BtnGetCommentAsync()
        {
            _likeSelectedComment = false;
            if (_taskModels != null)
            {
                _taskModels = null;
            }
            _commentModel = await GetCommentAsync(Name, int.Parse(_orderTyp.Item1));

            StateHasChanged();
        }
        private async Task BtnGetLikeCommentAsync()
        {
            _likeSelectedComment = true;
            if (_taskModels != null)
            {
                _taskModels = null;
            }

            _commentModel = await GetCommentAsync(null, int.Parse(_orderTyp.Item1));

            StateHasChanged();
        }
        private async Task ChangePageComment()
        {
            if (_likeSelectedComment)
                await BtnGetLikeCommentAsync();
            else
                await BtnGetCommentAsync();
        }

        private void BtnEditProfile()
        {
            editProfil = !editProfil;
        }
        #endregion

        public void Dispose()
        {
            _userPublicModel = null;
#if DEBUG
            _ilogger.LogInformation("Dispose");
#endif
        }
    }
}
