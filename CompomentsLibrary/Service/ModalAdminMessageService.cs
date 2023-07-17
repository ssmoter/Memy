using CompomentsLibrary.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompomentsLibrary.Service
{
    public class ModalAdminMessageService
    {
        public event Func<string?, string?, PopupLevel.Level, string?, string?, string?, Task<(string?, string?, int?)?>> OnShow;

        public async Task<(string?, string?, int?)?>? ShowPopup(string headerText = "",
                                     string bodyText = "",
                                     PopupLevel.Level level1 = PopupLevel.Level.None,
                                     string level2 = "none",
                                     string yesText = "Ok",
                                     string noText = "Cancel")
        {
            return await OnShow?.Invoke(headerText, bodyText, level1, level2, yesText, noText);
        }
    }
}
