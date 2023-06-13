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
        private string _date { get; set; }
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
            _date = CompareDate.GetDate(taskModel.CreatedDate);
            StateHasChanged();
        }
        private void ChangeImg(int index)
        {
            _maingImg = index;
        }
        #region Close

        private void Close()
        {
#if DEBUG
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
