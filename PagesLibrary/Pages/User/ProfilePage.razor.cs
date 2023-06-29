namespace PagesLibrary.Pages.User
{
    public partial class ProfilePage : IDisposable
    {


        protected override async Task OnInitializedAsync()
        {
            _userPublicModel = new Memy.Shared.Model.UserPublicModel
            {
                Avatar = "",
                CreatedDate = DateTime.Now,
                Id = 0,
                Nick = "",
                NumberOfTask = 10,
                SumTaskLike = 1,
                SumTaskUnLike = 3,
            };

            if (_userPublicModel != null)
            {
                decimal like = _userPublicModel.SumTaskLike;
                decimal unlike = _userPublicModel.SumTaskUnLike;

                _percentLike = Math.Round((like / unlike), 2);
                _createdDate = _userPublicModel.CreatedDate.Value.ToString("dd.MM.yyyy");
            }
        }


        public void Dispose()
        {
            _userPublicModel = null;
        }
    }
}
