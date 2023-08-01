using CompomentsLibrary.Helper;
using CompomentsLibrary.Model;
using CompomentsLibrary.Service;

using Microsoft.AspNetCore.Components;

using System.ComponentModel.DataAnnotations;

namespace CompomentsLibrary.Pages
{
    public partial class ModalAdminMessage
    {
        [Parameter]
        public bool IsVisible { get; set; }
        [Parameter]
        public EventCallback<bool> IsVisibleChanged { get; set; }
        [Parameter]
        public string? HeaderText { get; set; }
        [Parameter]
        public string? BodyText { get; set; }
        [Parameter]
        public string? YesText { get; set; }
        [Parameter]
        public string? NoText { get; set; }
        [Parameter]
        public EventCallback OnStateChanged { get; set; }




        private CancellationTokenSource? FinishConfirm;
        private ModalMessage message = new ModalMessage();
        private ValueInDropDownList[] listLevel = new ValueInDropDownList[]
        {
            new ValueInDropDownList(((int)PopupLevel.Level.None).ToString(),PopupLevel.Level.None.ToString()),
            new ValueInDropDownList(((int)PopupLevel.Level.Info).ToString(),PopupLevel.Level.Info.ToString()),
            new ValueInDropDownList(((int)PopupLevel.Level.Success).ToString(),PopupLevel.Level.Success.ToString()),
            new ValueInDropDownList(((int)PopupLevel.Level.Warning).ToString(),PopupLevel.Level.Warning.ToString()),
            new ValueInDropDownList(((int)PopupLevel.Level.Error).ToString(),PopupLevel.Level.Error.ToString()),
        };


        [Inject] ModalAdminMessageService? PopupService { get; set; }
        protected override void OnInitialized()
        {
            if (PopupService is null)
            {
                PopupService = new ModalAdminMessageService();
            }
            PopupService.OnShow += Show;
        }

        internal async virtual void RaiseChange()
        {
            await OnStateChanged.InvokeAsync();
        }
        public async Task<AdminModalResult?> Show(string headerText = "",
                                     string bodyText = "",
                                     PopupLevel.Level level = PopupLevel.Level.None,
                                     string level2 = "none",
                                     string yesText = "Ok",
                                     string noText = "Cancel")
        {
            message = new ModalMessage(headerText, bodyText, new ValueInDropDownList(((int)level).ToString(), level2));

            HeaderText = headerText;
            BodyText = bodyText;
            YesText = yesText;
            NoText = noText;
            IsVisible = true;

            StateHasChanged();
            try
            {
                using (FinishConfirm = new())
                {
                    await Task.Delay(-1, FinishConfirm.Token);
                }
            }
            catch (TaskCanceledException)
            { }
            return new AdminModalResult()
            {
                Header = message.Header,
                Body = message.Body,
                Level = (PopupLevel.Level)message.Level
            };
        }

        private void Close(bool value)
        {
            if (!value)
            {
                message = new();
                HeaderText = string.Empty;
                BodyText = string.Empty;
            }
            IsVisible = false;
            StateHasChanged();
            if (FinishConfirm is not null)
            {
                if (FinishConfirm.Token.CanBeCanceled)
                {
                    FinishConfirm.Cancel();
                }
            }
        }

        private class ModalMessage
        {
            public ModalMessage(string? header, string? body, ValueInDropDownList level)
            {
                Header = header;
                Body = body;
                messageLevel = level;
            }
            public ModalMessage()
            {
                messageLevel = new ValueInDropDownList("", "");
            }
            [Required(ErrorMessage = "Tytuł jest wymagany")]
            public string? Header { get; set; }
            [Required(ErrorMessage = "Treść jest wymagana")]
            public string? Body { get; set; }

            public ValueInDropDownList messageLevel { get; set; }
            public int Level
            {
                get
                {
                    if (!string.IsNullOrWhiteSpace(messageLevel.Item1))
                    {
                        return int.Parse(messageLevel.Item1);
                    }
                    return 0;
                }
            }
        }

    }
}
