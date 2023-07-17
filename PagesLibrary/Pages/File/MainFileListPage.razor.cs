using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages.File
{
    public partial class MainFileListPage : IDisposable
    {
        public TaskModel[]? TaskModels { get; set; }


        protected override async Task OnInitializedAsync()
        {
            await GetTaskAsync();
#if DEBUG
            _logger.LogInformation("Initialized page");
#endif
        }


        private async Task ChangeDateOrOrder()
        {
            await ChangePage(0);
        }
        private async Task ChangePage(int index)
        {
            if (Categories != null)
            {
                if (Banned != null)
                {
                    _navigation.NavigateTo($"/directory/{Categories}/{index}/{Banned}");
                }
                else
                {
                    _navigation.NavigateTo($"/directory/{Categories}/{index}");
                }
            }
            else
            {
                _navigation.NavigateTo($"/{index}");
            }
            Start = index;
            await GetTaskAsync();
        }

        private async Task GetTaskAsync()
        {
            try
            {
                if (Start == null)
                {
                    Start = 0;
                }
                DateEnd = _dateLenght.Item1;
                int order = int.Parse(_orderTyp.Item1);
                var result = await _iFileManager.GetTaskModelsAsync(Start, Categories, Max, Banned, DateEnd, DateStart, order);
                if (result.IsSuccessStatusCode)
                {
                    TaskModels = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskModel[]>(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    _popUp.ShowToats("Nie udało się pobrać danych", "Warning", CompomentsLibrary.Helper.PopupLevel.Level.Warning);
                    _logger.LogError(await result.Content.ReadAsStringAsync());
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _popUp.ShowToats("Nie udało się pobrać danych", "Error", CompomentsLibrary.Helper.PopupLevel.Level.Error);

            }
        }



        public void Dispose()
        {
            TaskModels = null;
        }
    }
}
