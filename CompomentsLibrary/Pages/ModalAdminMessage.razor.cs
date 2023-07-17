using CompomentsLibrary.Helper;
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




        private CancellationTokenSource FinishConfirm;
        private ModalMessage message = new ModalMessage();
        private (string, string)[] listLevel = new (string, string)[]
        {
            (((int)PopupLevel.Level.None).ToString(),PopupLevel.Level.None.ToString()),
            (((int)PopupLevel.Level.Info).ToString(),PopupLevel.Level.Info.ToString()),
            (((int) PopupLevel.Level.Success).ToString(),PopupLevel.Level.Success.ToString()),
            (((int) PopupLevel.Level.Warning).ToString(),PopupLevel.Level.Warning.ToString()),
            (((int) PopupLevel.Level.Error).ToString(),PopupLevel.Level.Error.ToString()),
        };


        [Inject] ModalAdminMessageService PopupService { get; set; }
        protected override void OnInitialized()
        {
            PopupService.OnShow += Show;
        }

        internal async virtual void RaiseChange()
        {
            await OnStateChanged.InvokeAsync();
        }
        public async Task<(string?, string?, int?)?> Show(string headerText = "",
                                     string bodyText = "",
                                     PopupLevel.Level level = PopupLevel.Level.None,
                                     string level2 = "none",
                                     string yesText = "Ok",
                                     string noText = "Cancel")
        {
            message = new ModalMessage(headerText, bodyText, (((int)level).ToString(), level2));


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
            return (message.Header, message.Body, message.Level);
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
            if (FinishConfirm.Token.CanBeCanceled)
            {
                FinishConfirm.Cancel();
            }
        }

        private class ModalMessage
        {
            public ModalMessage(string? header, string? body, (string, string) level)
            {
                Header = header;
                Body = body;
                messageLevel = level;
            }
            public ModalMessage()
            {

            }
            [Required(ErrorMessage = "Tytuł jest wymagany")]
            public string? Header { get; set; }
            [Required(ErrorMessage = "Treść jest wymagana")]
            public string? Body { get; set; }

            /// <summary>
            /// (string,string) pierszy string jest int opisującym poziom popUp a drugi jego opisem
            /// </summary>
            public (string, string) messageLevel { get; set; }
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
