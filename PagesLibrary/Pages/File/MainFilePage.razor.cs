using CompomentsLibrary.Helper;

using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

using PagesLibrary.Data;

namespace PagesLibrary.Pages.File
{
    public partial class MainFilePage : IDisposable
    {
        private int _descriptionLength = 300;
        private string? _date { get; set; }
        private int _maingImg { get; set; } = 0;

        protected override async Task OnParametersSetAsync()
        {
            if (TaskModel is not null)
            {
                if (TaskModel.Id != Id && Id != 0)
                {
                    await InitializedTask();
                }

                if (TaskModel != null)
                {
                    _date = CompareDate.GetDate(TaskModel.CreatedDate);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (TaskModel is null)
            {
                await InitializedTask();
            }

            if (TaskModel != null)
            {
                _date = CompareDate.GetDate(TaskModel.CreatedDate);
            }
#if DEBUG

            int number = 0;
            if (TaskModel != null)
            {
                number = TaskModel.Id;
            }

            _logger.LogInformation("InitializedAsync page {0}", number);
#endif
        }

        private async Task InitializedTask()
        {
            var result = await _iFileManager.GetTaskModelAsync(Id);
            var json = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                TaskModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskModel>(json);
            }
            else
            {
                _logger.LogError(json);
            }
        }

        private async Task SetReaction(int id, int value)
        {
            try
            {
                var result = await _reaction.SetReaction(id, value, Memy.Shared.Helper.MyEnums.TypOfReaction.File);

                if (result != null)
                {
                    _popUp.ShowToats("Reakcja dodana", "Dodawanie reakcji", PopupLevel.Level.Success, 2);
                    ArgumentNullException.ThrowIfNull(TaskModel);
                    TaskModel.Reaction = result;

                }
                else
                {
                    _popUp.ShowToats("Wystąpił błąd, spróbuj ponownie", "Dodawanie reakcji", PopupLevel.Level.Warning, 2);
                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Wystąpił błąd, spróbuj ponownie", "Dodawanie reakcji", PopupLevel.Level.Error, 2);
                _logger.LogError(ex.Message);
            }
        }
        private ReadOnlySpan<char> GetFirstSegment(ReadOnlySpan<char> value)
        {
            if (value.Length > _descriptionLength)
            {
                return value.Slice(0, _descriptionLength);
            }
            else
            {
                return value.Slice(0, value.Length);
            }
        }
        private ReadOnlySpan<char> GetRestSegment(ReadOnlySpan<char> value)
        {
            if (value.Length > _descriptionLength)
            {
                return value.Slice(_descriptionLength, value.Length - _descriptionLength);
            }
            else
            {
                return null;
            }
        }
        private void ChangeImg(int index)
        {
            _maingImg = index;
        }

        private void ImgLeft()
        {
            if (TaskModel is null)
            {
                return;
            }
            if (TaskModel.FileModel is null)
            {
                return;
            }
            if (_maingImg > 0)
            {
                _maingImg--;
            }
            else
            {
                _maingImg = TaskModel.FileModel.Length - 1;
            }
        }
        private void ImgRight()
        {
            if (TaskModel is null)
            {
                return;
            }
            if (TaskModel.FileModel is null)
            {
                return;
            }
            if (_maingImg < TaskModel.FileModel.Length - 1)
            {
                _maingImg++;
            }
            else
            {
                _maingImg = 0;
            }
        }
        public void Dispose()
        {
            TaskModel = null;
        }
    }
}
