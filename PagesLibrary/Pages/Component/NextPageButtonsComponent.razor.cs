namespace PagesLibrary.Pages.Component
{
    public partial class NextPageButtonsComponent
    {
        private async Task ChangePage(int index)
        {
            Start = index;
            OnClick?.Invoke(index);
        }


    }
}
