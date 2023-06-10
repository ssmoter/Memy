using CompomentsLibrary.Helper;

using Microsoft.Extensions.Logging;

using PagesLibrary.Data;

namespace PagesLibrary.Pages
{
    public partial class MainFilePage : IDisposable
    {
        private int _descriptionLength = 300;
        private string _date { get; set; }
        private int _maingImg { get; set; } = 0;

        protected override void OnInitialized()
        {
            _date = CompareDate.GetDate(TaskModel.CreatedDate);
#if DEBUG
            _logger.LogInformation("Initialized page");
#endif
        }

        private async Task SetReaction(int id, int value)
        {
            try
            {
                var result = await _reaction.SetReaction(id, value, Memy.Shared.Helper.MyEnums.TypOfReaction.File);

                if (result != null)
                {
                    _popUp.ShowToats("Reakcja dodana", "Dodawanie reakcji", PopupLevel.Level.Success);
                    TaskModel.Reaction = result;

                }
                else
                {
                    _popUp.ShowToats("Wystąpił błąd, spróbuj ponownie", "Dodawanie reakcji", PopupLevel.Level.Warning);
                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Wystąpił błąd, spróbuj ponownie", "Dodawanie reakcji", PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }
        private ReadOnlySpan<char> GetFirstSegment(ReadOnlySpan<char> value)
        {
            if (value.Length > _descriptionLength)
            {
                return value.Slice(0, _descriptionLength);
            }
            else
            {
                return value.Slice(0, value.Length);
            }
        }
        private ReadOnlySpan<char> GetRestSegment(ReadOnlySpan<char> value)
        {
            if (value.Length > _descriptionLength)
            {
                return value.Slice(_descriptionLength, value.Length - _descriptionLength);
            }
            else
            {
                return null;
            }
        }
        private void ChangeImg(int index)
        {
            _maingImg = index;
        }


        public void Dispose()
        {
            TaskModel = null;
        }
    }
}
