using Memy.Shared.Model;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using PagesLibrary.Data;
using PagesLibrary.Service;

namespace PagesLibrary.Pages.File
{
    public partial class MainFilePopUp : IDisposable
    {

        private bool _isInside { get; set; }
        private string? _date { get; set; }
        private int _maingImg { get; set; } = 0;

        [Inject] MainFilePopUpService? mainFilePopUpService { get; set; }

        protected override void OnInitialized()
        {
            if (mainFilePopUpService != null)
            {
                mainFilePopUpService.OnShow += DisplaySingleFile;
            }
#if DEBUG
            _logger.LogInformation("Initialized page");
#endif
        }
        public void DisplaySingleFile(TaskModel? taskModel)
        {
            TaskModel = taskModel;
            IsVisible = true;
            if (taskModel is not null)
            {
                _date = CompareDate.GetDate(taskModel.CreatedDate);
            }
            StateHasChanged();
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


        #region Close

        private void Close()
        {
#if DEBUG
            if(TaskModel is not null) 
                _logger.LogInformation("Close {0}", TaskModel.Id);
#endif
            TaskModel = null;
            IsVisible = false;
            StateHasChanged();
        }
        private void MouseClose()
        {
            if (!_isInside && IsVisible)
            {
                Close();
            }
        }
        private void MouseOut()
        {
            _isInside = false;
        }
        private void MouseEnter()
        {
            _isInside = true;
        }
        #endregion
        public void Dispose()
        {
            TaskModel = null;
        }

    }
}
