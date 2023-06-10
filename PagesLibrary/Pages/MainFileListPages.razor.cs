﻿using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages
{
    public partial class MainFileListPages : IDisposable
    {
        public TaskModel[]? TaskModels { get; set; }

        private async Task ChangePage(int index)
        {
            if (Categories != null)
            {
                if (Banned != null)
                {
                    _navigation.NavigateTo($"/{Categories}/{index}/{Banned}");
                }
                else
                {
                    _navigation.NavigateTo($"/{Categories}/{index}");
                }
            }
            else
            {
                _navigation.NavigateTo($"/{index}");
            }
            Start = index;
            await GetTaskAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            await GetTaskAsync();
        }

        private async Task GetTaskAsync()
        {
            try
            {
                if (Start == null)
                {
                    Start = 1;
                }
#if DEBUG
                _logger.LogInformation("Initialized page");
#endif
                var result = await _iFileManager.GetTaskModelsAsync(Start, Categories, Max, Banned, DateEnd, DateStart);
                if (result.IsSuccessStatusCode)
                {
                    TaskModels = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskModel[]>(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    _logger.LogError(await result.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public void Dispose()
        {
            TaskModels = null;
        }
    }
}
