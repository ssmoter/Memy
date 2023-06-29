using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

using PagesLibrary.Service;

namespace PagesLibrary.Pages.File
{
    public partial class CategoriesPopUp
    {
        private bool _isInside;
        private const string directory = "/directory/";
        [Inject] CategoriesPopUpServie? categoriesPopUpServie { get; set; }
        protected override void OnInitialized()
        {
            if (categoriesPopUpServie != null)
            {
                categoriesPopUpServie.OnShow += Display;
            }
#if DEBUG
            _logger.LogInformation("Initialized page");
#endif
        }

        public void Display ()
        {
            IsVisible = true;
            StateHasChanged();
#if DEBUG
            _logger.LogInformation("Open directory");
#endif
        }

        #region Close

        private void Close()
        {
#if DEBUG
            _logger.LogInformation("Close directory");
#endif
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
    }
}
