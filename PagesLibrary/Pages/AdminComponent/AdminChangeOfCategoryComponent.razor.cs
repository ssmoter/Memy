using CompomentsLibrary.Helper;

using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

using PagesLibrary.Data.Admin;
using PagesLibrary.Helper;

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

            var url = _navigationManager.Uri;

            for (int i = 0; i < ListInDropDown.CategoriesTablePlusMain.Length; i++)
            {
                if (url.Contains(ListInDropDown.CategoriesTablePlusMain[i].Item1))
                {
                    categories = ListInDropDown.CategoriesTablePlusMain[i];
                    break;
                }
            }
        }


        ReportedMessagesModel? repored;
        string title = "";
        private async Task BtnMain()
        {
            try
            {
                ArgumentNullException.ThrowIfNull(Model);
                if (!string.IsNullOrWhiteSpace(Model.Title))
                {
                    title = Model.Title;
                }


                var result = await _adminModal.ShowPopup(title,
                                             "Post został dodany do głównej",
                                             PopupLevel.Level.Success,
                                             PopupLevel.Level.Success.ToString(),
                                             "Przenieś",
                                             "Anuluj");

                ArgumentNullException.ThrowIfNull(result);
                if (string.IsNullOrWhiteSpace(result.Header))
                {
                    return;
                }
                repored = new ReportedMessagesModel
                {
                    Header = result.Header,
                    Body = result.Body,
                    Level = (int)result.Level
                };

                await UpdateCategory(Memy.Shared.Helper.Categories.Main);
            }
            catch (Exception ex)
            {
                _popUp.ShowToats(ex.Message, "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }

        private async Task BtnCategory()
        {
            try
            {
                ArgumentNullException.ThrowIfNull(Model);
                if (!string.IsNullOrWhiteSpace(Model.Title))
                {
                    title = Model.Title;
                }

                var category = categories.Item1;
                var result = await _adminModal.ShowPopup(title,
                                             $"Post został przeniesiony na: {categories.Item2}",
                                             PopupLevel.Level.Success,
                                             PopupLevel.Level.Success.ToString(),
                                             "Przenieś",
                                             "Anuluj");

                ArgumentNullException.ThrowIfNull(result);
                if (string.IsNullOrWhiteSpace(result.Header))
                {
                    return;
                }
                repored = new ReportedMessagesModel
                {
                    Header = result.Header,
                    Body = result.Body,
                    Level = (int)result.Level
                };
                if (!string.IsNullOrWhiteSpace(category))
                {
                    await UpdateCategory(category);
                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats(ex.Message, "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }


        private async Task UpdateCategory(string category)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(Model);
                ArgumentNullException.ThrowIfNull(_adminApi);
                ArgumentNullException.ThrowIfNull(repored);

                var response = await _adminApi.UpdateCategory(Model.Id, category, repored);
                var json = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    _popUp.ShowToats($"Dany {Type.ToString()} został przeniesiony", "Success", CompomentsLibrary.Helper.PopupLevel.Level.Success);
                    if (Fucn != null)
                    {
                        Fucn?.Invoke();
                    }
                }
                else
                {
                    _popUp.ShowToats($"Nie udało się przenieść {Type.ToString()} ", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                    _popUp.ShowToats(json, "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning, 10);

                    _logger.LogWarning(json);
                }

            }
            catch (Exception )
            {
                throw;
            }
        }



        public void Dispose()
        {
            _adminApi = null;
        }




    }
}
