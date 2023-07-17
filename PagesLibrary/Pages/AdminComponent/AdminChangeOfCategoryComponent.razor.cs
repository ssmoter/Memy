using CompomentsLibrary.Helper;

using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

using PagesLibrary.Data.Admin;

namespace PagesLibrary.Pages.AdminComponent
{
    public partial class AdminChangeOfCategoryComponent : IDisposable
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

        private async Task BtnMain()
        {
            var result = await _adminModal.ShowPopup(Model.Title,
                                         "Post został dodany do głównej",
                                         PopupLevel.Level.Success,
                                         PopupLevel.Level.Success.ToString(),
                                         "Przenieś",
                                         "Anuluj");

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

            await UpdateCategory(Memy.Shared.Helper.Categories.Main);
        }

        private async Task BtnCategory()
        {
            var category = categories.Item1;
            var result = await _adminModal.ShowPopup(Model.Title,
                                         $"Post został przeniesiony na: {categories.Item2}",
                                         PopupLevel.Level.Success,
                                         PopupLevel.Level.Success.ToString(),
                                         "Przenieś",
                                         "Anuluj");

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

            await UpdateCategory(category);
        }


        private async Task UpdateCategory(string category)
        {
            try
            {
                var response = await _adminApi.UpdateCategory(Model.Id, category, repored);
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
