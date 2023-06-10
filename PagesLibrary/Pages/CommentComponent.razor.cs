using CompomentsLibrary.Helper;

using Memy.Shared.Model;

using Microsoft.Extensions.Logging;

using System.ComponentModel.DataAnnotations;

namespace PagesLibrary.Pages
{
    public partial class CommentComponent : IDisposable
    {

        #region Override
        protected override async Task OnInitializedAsync()
        {
#if DEBUG
            _logger.LogInformation("InitializedAsync");
#endif
            await GetComment(Id, _orderTyp);
        }

        protected override void OnInitialized()
        {
            //pobranie OrderTyp z localstorage
#if DEBUG
            _logger.LogInformation("Initialized");
#endif
        }
        #endregion

        public async Task GetComment(int id, int orderTyp)
        {
            try
            {
                var result = await _commentApi.GetCommentAsync(id, orderTyp);
                var json = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    _commentModel = Newtonsoft.Json.JsonConvert.DeserializeObject<CommentModel[]>(json);
                    if (orderTyp == 0 || orderTyp == 1)
                    {
                        dateTyp = !dateTyp;
                    }
                    if (orderTyp == 2 || orderTyp == 3)
                    {
                        popTyp = !popTyp;
                    }
                }
                else
                {
                    if (json != "Sequence contains no elements")
                    {
                        _popUp.ShowToats("Nie udało się pobrać komentarzy", "Wystąpił błąd", PopupLevel.Level.Warning);
                    }
                    _logger.LogWarning(json);
                }
            }
            catch (Exception ex)
            {
                _popUp.ShowToats("Nie udało się pobrać komentarzy", "Wystąpił błąd", PopupLevel.Level.Error);
                _logger.LogError(ex.Message);
            }
        }

        private async Task SetReaction(int id, int value)
        {
            //try
            //{
            //    var result = await _reaction.SetReaction(id, value, Memy.Shared.Helper.MyEnums.TypOfReaction.File);

            //    if (result != null)
            //    {
            //        _popUp.ShowToats("Reakcja dodana", "Dodawanie reakcji", PopupLevel.Level.Success);
            //        var model = TaskModels.FirstOrDefault(x => x.Id == id);
            //        if (model != null)
            //        {
            //            model.Reaction[0] = result;
            //        }
            //    }
            //    else
            //    {
            //        _popUp.ShowToats("Wystąpił błąd, spróbuj ponownie", "Dodawanie reakcji", PopupLevel.Level.Warning);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _popUp.ShowToats("Wystąpił błąd, spróbuj ponownie", "Dodawanie reakcji", PopupLevel.Level.Error);
            //    _logger.LogError(ex.Message);
            //}
        }
        public async Task OnValidSubmit()
        {
            if (!(await _modal.ShowPopup("Przesyłanie", "Czy na pewno chcesz przesłać komentarz?", "Tak", "Nie")))
            {
                return;
            }


        }

        public void Dispose()
        {
            _commentModel = null;
            NewComment = null;
        }

        class CommentAdd
        {
            [Required(ErrorMessage ="Wiadomość jest wymagana")]
            [MinLength(length: 3, ErrorMessage = "Wiadomość jest za krótka")]
            [MaxLength(length: 100, ErrorMessage = "Wiadomość jest zadługa")]
            public string? Description { get; set; }
        }
    }
}
