using CompomentsLibrary.Helper;

using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages.Comment
{
    public partial class CommentComponent : IDisposable
    {
        private int _descriptionLength = 300;

        private async Task SetReaction(int id, int value)
        {
            try
            {
                var result = await _reaction.SetReaction(id, value, Memy.Shared.Helper.MyEnums.TypOfReaction.Comment);

                if (result != null)
                {
                    _popUp.ShowToats("Reakcja dodana", "Dodawanie reakcji", PopupLevel.Level.Success, 1);
                    CommentModel.Reaction = result;
                }
                else
                {
                    _popUp.ShowToats("Wystąpił błąd, spróbuj ponownie", "Dodawanie reakcji", PopupLevel.Level.Warning, 1);
                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Wystąpił błąd, spróbuj ponownie", "Dodawanie reakcji", PopupLevel.Level.Error, 1);
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

        public void Dispose()
        {
            CommentModel = null;
        }


    }
}
