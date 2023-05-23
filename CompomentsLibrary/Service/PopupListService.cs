using CompomentsLibrary.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompomentsLibrary.Service
{
    public class PopupListService
    {
        public event Action<string?, string?, PopupLevel.Level?>? OnShow;

        public void ShowToats(string? bodyText, string? headerText = "", PopupLevel.Level? level = null)
        {
            OnShow?.Invoke(bodyText, headerText, level);
        }
    }
}
