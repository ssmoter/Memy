using CompomentsLibrary.Helper;

using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

using PagesLibrary.Data.Admin;

namespace PagesLibrary.Pages.AdminComponent
{
    public partial class AdminBanButtonComponent
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
        ReportedMessagesModel repored;

        public async Task Ban()
        {
            try
            {
                string? body = "";
                if (Model.Banned)
                {
                    body = "Post został odbanowany";
                }
                else
                {
                    body = "Post zostal zbanowany zapoznaj sie z regulaminem w celu jego przywrócenia";
                }

                var result = await _adminModal.ShowPopup(Model.Title,
                                                         bodyText: body,
                                                         level1: PopupLevel.Level.Warning,
                                                         level2: PopupLevel.Level.Warning.ToString(),
                                                         yesText: "Ban",
                                                         noText: "Anuluj");

                if (string.IsNullOrWhiteSpace(result.Value.Item1))
                {
                    return;
                }
                repored = new ReportedMessagesModel
                {
                    Header = result.Value.Item1,
                    Body = result.Value.Item2,
                    Level = (int)result.Value.Item3
                };

                var response = await _adminApi.Ban(Model.Id, repored);
                var json = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _popUp.ShowToats($"Dany {Type.ToString()} został zbanowany", "Success", CompomentsLibrary.Helper.PopupLevel.Level.Success);
                    if (Fucn != null)
                    {
                        Fucn?.Invoke();
                    }
                }
                else
                {
                    _popUp.ShowToats($"Nie udało się zbanować {Type.ToString()} ", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
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

    }
}
