namespace PagesLibrary.Pages.Component
{
    public partial class NextPageButtonsComponent
    {
        private void ChangePage(int index)
        {
            Start = index;
            OnClick?.Invoke(index);
        }


    }
}
