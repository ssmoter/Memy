using CompomentsLibrary.Helper;
using CompomentsLibrary.Service;

using Microsoft.AspNetCore.Components;

namespace CompomentsLibrary.Pages
{
    public partial class PopupList : ComponentBase, IDisposable
    {
        private System.Timers.Timer? timer;
        private List<Toasts>? toasts;
        [Inject] PopupListService? popupListService { get; set; }

        protected override void OnInitialized()
        {
            if (popupListService is not null)
            {
                popupListService.OnShow += Show;
            }

            if (toasts == null)
            {
                toasts = new List<Toasts>();
            }
            if (timer == null)
            {
                timer = new System.Timers.Timer(1000);
                timer.Elapsed += (sender, args) => ElapsedTimer(sender, args);
            }
        }
        private void ElapsedTimer(object? source, System.Timers.ElapsedEventArgs e)
        {
            if (toasts is not null)
            {
                for (int i = 0; i < toasts.Count; i++)
                {
                    toasts[i].Time--;
                    if (toasts[i].Time == 0)
                    {
                        Close(i);
                    }
                }
            }
        }

        public void Show(string bodyText = "", string headerText = "", PopupLevel.Level level = PopupLevel.Level.None, int time = 5)
        {
            if (toasts is null)
            {
                toasts = new List<Toasts>();
            }

            toasts.Add(new Toasts()
            {
                BodyText = bodyText,
                HeaderText = headerText,
                Level = level,
                IsVisible = true,
                Time = time
            });

            ToatsLevel(level);
            if (timer is null)
            {
                timer = new System.Timers.Timer(1000);
            }
            timer.Start();
            StateHasChanged();
        }

        private void Close(int index = 0)
        {
            if (toasts is not null)
            {

                if (toasts.Count <= 0)
                {
                    if (timer is not null)
                    {
                        timer.Stop();
                    }
                }
                toasts.RemoveAt(index);
            }
            StateHasChanged();
        }
        private void ToatsLevel(PopupLevel.Level? level)
        {
            if (level == null)
            {
                level = PopupLevel.Level.None;
            }
            if (toasts is null)
                toasts = new List<Toasts>();

            switch (level)
            {
                case PopupLevel.Level.None:
                    {
                        toasts.Last().BackgroundCssClass = $"bg-primary";
                    }
                    break;
                case PopupLevel.Level.Info:
                    {
                        toasts.Last().BackgroundCssClass = $"bg-info";
                    }
                    break;
                case PopupLevel.Level.Success:
                    {
                        toasts.Last().BackgroundCssClass = $"bg-success";
                    }
                    break;
                case PopupLevel.Level.Warning:
                    {
                        toasts.Last().BackgroundCssClass = $"bg-warning";
                    }
                    break;
                case PopupLevel.Level.Error:
                    {
                        toasts.Last().BackgroundCssClass = "bg-danger";
                    }
                    break;
            }
        }

        public void Dispose()
        {
            toasts = null;
            timer = null;
        }

        private class Toasts
        {
            public bool IsVisible { get; set; }
            public string? HeaderText { get; set; }
            public string? BodyText { get; set; }
            public PopupLevel.Level? Level { get; set; } = PopupLevel.Level.None;
            public string? BackgroundCssClass { get; set; }
            public int Time { get; set; } = 5;
        }
    }
}
