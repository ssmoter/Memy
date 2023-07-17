using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages.Component
{
    public partial class ReportButtonComponent
    {
        public async Task ReportBtn()
        {
            try
            {
                Reported.Id = Id;
                if (Reported.Value == 1)
                {
                    Reported.Value = 0;
                }
                else
                {
                    Reported.Value = 1;
                }

                var result = await _reportedApi.SetReported(Reported);

                if (result != null)
                {
                    if (result.IsChecked)
                    {
                        _popUp.ShowToats("Zgłoszenie zostało przyjęte", "Zgłoszenie", CompomentsLibrary.Helper.PopupLevel.Level.Info);
                    }
                    else
                    {
                        _popUp.ShowToats("Zgłoszenie zostało anulowane", "Zgłoszenie", CompomentsLibrary.Helper.PopupLevel.Level.Info);
                    }
                    Reported = result;
                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Wystąpił błąd przy zgłaszaniu", "Zgłoszenie", CompomentsLibrary.Helper.PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }

    }
}
