using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages.User
{
    public partial class UserMessagesComponent
    {
        protected override async Task OnInitializedAsync()
        {
            var user = await _auth.GetAuthenticationStateAsync();
            if (user.User.Identity is null)
            {
                return;
            }

            if (user.User.Identity.Name is not null)
            {
                await GetMessages();
            }
        }
        private async Task GetMessages()
        {
            try
            {
                messages = await _messagesApi.GetReportedMessages();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task BtnGetMessages()
        {
            IsVisible = !IsVisible;
            if (IsVisible)
            {
                await GetMessages();
            }
        }
        private async Task BtnRemoveMessages(int index)
        {
            if (messages is not null)
            {
                if (messages[index].BeenDelete == false)
                {
                    messages[index].BeenDelete = true;
                    await UpdateMessages(messages[index]);
                }
            }
        }

        private async Task IsChecked(int index)
        {
            if (messages is not null)
            {
                if (messages[index].BeenChecked == false)
                {
                    messages[index].BeenChecked = true;
                    await UpdateMessages(messages[index]);
                }
            }
        }

        private async Task UpdateMessages(ReportedMessagesModel model)
        {
            try
            {
                messages = await _messagesApi.PutReportedMessages(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
