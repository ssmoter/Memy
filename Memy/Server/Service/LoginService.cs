using Memy.Server.Data.User;
using Memy.Server.TokenAuthentication;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class LoginService
    {
        private readonly IUserData _userData;
        private readonly ITokenManager _tokenManager;
        private readonly ILogger _logger;

        public LoginService(IUserData userData, ITokenManager tokenManager, ILogger logger)
        {
            _userData = userData;
            _tokenManager = tokenManager;
            _logger = logger;
        }

        //tworzenie i dodanie tokena przy logowaniu
        public async Task<UserStorage> SetToken(UserSimple value)
        {
            try
            {
                var user = await _userData.LogIn<LoginUser>(value.Email, value.Password, value.Token.DoNotLogOut);
                if (user.Id == 0)
                {
                    return null;
                }
                var result = new UserStorage();
                LoginUser.UserSimpleParse(user, result);
                //_tokenManager.NewToken(new Token()
                //{
                //    Value = user.Value,
                //    ExpiryDate = user.ExpiryDate
                //});

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        //usuwanie tokena 
        //wylogowanie
        public async Task<string> DeleteToken(string token)
        {
            try
            {
                await _userData.LogOut(token);

                _tokenManager.DeleteToken(token);

                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }



        class LoginUser
        {
            public int? Id { get; set; }
            public string? Nick { get; set; }
            public Guid? Value { get; set; }
            public DateTimeOffset? ExpiryDate { get; set; }
            public bool? DoNotLogOut { get; set; }
            public string? Role { get; set; }
            public static void UserSimpleParse(LoginUser? loginUser, UserStorage? storage)
            {
                storage.UserName = loginUser.Nick;
                storage.Token = loginUser.Value.ToString();
                storage.Role = loginUser.Role;
            }
        }
    }

}
