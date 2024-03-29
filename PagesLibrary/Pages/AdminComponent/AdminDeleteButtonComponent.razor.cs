﻿using CompomentsLibrary.Helper;

using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

using PagesLibrary.Data.Admin;

namespace PagesLibrary.Pages.AdminComponent
{
    public partial class AdminDeleteButtonComponent : IDisposable
    {
        private IAdminApi? _adminApi;
        protected override void OnInitialized()
        {
            switch (Type)
            {
                case Memy.Shared.Helper.MyEnums.AdminDeleteBanType.AnswerComment:
                    break;
                case Memy.Shared.Helper.MyEnums.AdminDeleteBanType.Comment:
                    _adminApi = _AdminComment;
                    break;
                case Memy.Shared.Helper.MyEnums.AdminDeleteBanType.File:
                    _adminApi = _AdminFile;

                    break;
                case Memy.Shared.Helper.MyEnums.AdminDeleteBanType.Profile:
                    break;
                default:
                    break;
            }
        }


        ReportedMessagesModel? repored;
        public async Task Delete()
        {
            try
            {
                string title = "";
                ArgumentNullException.ThrowIfNull(Model);
                if (!string.IsNullOrWhiteSpace(Model.Title))
                {
                    title = Model.Title;
                }
                var result = await _adminModal.ShowPopup(title,
                                                         "Post został usunięty",
                                                         PopupLevel.Level.Warning,
                                                         PopupLevel.Level.Warning.ToString(),
                                                         "Usuń",
                                                         "Anuluj");
                if (result is null)
                {
                    return;
                }
                repored = new ReportedMessagesModel
                {
                    Header = result.Header,
                    Body = result.Body,
                    Level = (int)result.Level
                };


                ArgumentNullException.ThrowIfNull(_adminApi);
                var response = await _adminApi.Delete(Model.Id, repored);
                var json = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _popUp.ShowToats($"Dany {Type.ToString()} został usunięty", "Success", CompomentsLibrary.Helper.PopupLevel.Level.Success);
                    if (Fucn != null)
                    {
                        Fucn?.Invoke();
                    }
                }
                else
                {
                    _popUp.ShowToats($"Nie udało się usunąć {Type.ToString()} ", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                    _popUp.ShowToats(json, "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning, 10);

                    _logger.LogWarning(json);
                }

            }
            catch (Exception ex)
            {
                _popUp.ShowToats(ex.Message, "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }

        public void Dispose()
        {
            _adminApi = null;
        }
    }
}
