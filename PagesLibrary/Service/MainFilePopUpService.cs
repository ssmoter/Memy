using Memy.Shared.Model;

namespace PagesLibrary.Service
{
    public class MainFilePopUpService
    {
        public event Action<TaskModel?>? OnShow;
        public void ShowPopUp(TaskModel model)
        {
            OnShow?.Invoke(model);
        }
    }
}
