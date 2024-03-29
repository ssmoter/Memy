﻿using CompomentsLibrary.Service;

using Microsoft.AspNetCore.Components;

namespace CompomentsLibrary.Pages
{
    public partial class ModalPopUp
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
        private CancellationTokenSource? FinishConfirm;
        private bool IsInside { get; set; }
        [Inject] ModalPopUpService? PopupService { get; set; }
        protected override void OnInitialized()
        {
            if (PopupService is null)
            {
                PopupService = new ModalPopUpService();
            }
            PopupService.OnShow += Show;
        }
        public bool Result { get; set; }
        [Parameter]
        public EventCallback OnStateChanged { get; set; }
        internal async virtual void RaiseChange()
        {
            await OnStateChanged.InvokeAsync();
        }
        public async Task<bool> Show(string headerText = "", string bodyText = "", string yesText = "Ok", string noText = "Cancel")
        {
            HeaderText = headerText;
            BodyText = bodyText;
            YesText = yesText;
            NoText = noText;
            IsVisible = true;
            IsInside = true;
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
            return Result;
        }

        private void Close(bool value)
        {
            Result = value;
            HeaderText = string.Empty;
            BodyText = string.Empty;
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

        private void MouseClose()
        {
            if (!IsInside)
            {
                Close(false);
            }
        }
        private void MouseOut()
        {
            IsInside = false;
        }
        private void MouseEnter()
        {
            IsInside = true;
        }
    }
}
