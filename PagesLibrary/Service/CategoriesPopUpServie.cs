using Memy.Shared.Model;

namespace PagesLibrary.Service
{
    public class CategoriesPopUpServie
    {
        public event Action OnShow;
        public void ShowPopUp()
        {
            OnShow?.Invoke();
        }
    }
}
