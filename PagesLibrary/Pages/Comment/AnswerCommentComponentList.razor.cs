using CompomentsLibrary.Helper;

using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

using System.ComponentModel.DataAnnotations;

namespace PagesLibrary.Pages.Comment
{
    public partial class AnswerCommentComponentList
    {
        protected override async Task OnInitializedAsync()
        {
#if DEBUG
            _logger.LogInformation("InitializedAsync");
#endif
            await GetComment(Id, OrderTyp);
        }

        public async Task GetComment(int id, int orderTyp)
        {
            try
            {
                var result = await _commentApi.GetAnswerCommentAsync(id, orderTyp);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    _commentModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommentModel[]>(json);
                }
                else
                {
                    _popUp.ShowToats("Nie udało się pobrać komentarzy", "Wystąpił błąd", PopupLevel.Level.Warning);
                    _logger.LogWarning(json);
                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Nie udało się pobrać komentarzy", "Wystąpił błąd", PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }
        public async Task OnValidSubmit()
        {
            if (!await _modal.ShowPopup("Przesyłanie", "Czy na pewno chcesz przesłać komentarz?", "Tak", "Nie"))
            {
                return;
            }
            try
            {
                var comm = new Memy.Shared.Model.Comment()
                {
                    Description = NewComment.Description,
                    ObjectId = Id,
                };

                var result = await _commentApi.SendAnswerCommentAsync(comm, OrderTyp);
                var json = await result.Content.ReadAsStringAsync();

                if (result.IsSuccessStatusCode)
                {
                    NewComment = new CommentAdd();
                    _commentModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommentModel[]>(json);
                }
                else
                {
                    _popUp.ShowToats("Nie udało się wysłać komentarza", "Wystąpił błąd", PopupLevel.Level.Warning);
                    _logger.LogWarning(json);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }


        }


        class CommentAdd
        {
            [Required(ErrorMessage = "Wiadomość jest wymagana")]
            [MinLength(length: 3, ErrorMessage = "Wiadomość jest za krótka")]
            [MaxLength(length: 20000, ErrorMessage = "Wiadomość jest zadługa")]
            public string? Description { get; set; }
        }
    }
}
