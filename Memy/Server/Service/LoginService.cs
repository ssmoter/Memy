using Memy.Server.Data;
using Memy.Server.Data.User;
using Memy.Server.TokenAuthentication;
using Memy.Shared.Helper;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class LoginService
    {
        private readonly ILoginData _userData;
        private readonly ITokenManager _tokenManager;

        public LoginService(ILoginData userData, ITokenManager tokenManager)
        {
            _userData = userData;
            _tokenManager = tokenManager;
        }

        //tworzenie i dodanie tokena przy logowaniu
        public async Task<string?> SetToken(UserSimple value)
        {
            try
            {
                LoginUser? user = null;
                if (!string.IsNullOrWhiteSpace(value.Email) && !string.IsNullOrWhiteSpace(value.Password))
                {
                    var pass = ConvertByteString.ConvertToObject<string>(value.Password);
                    ArgumentNullException.ThrowIfNullOrEmpty(pass);
                    user = await _userData.LogIn<LoginUser>(value.Email, pass, value.Token.DoNotLogOut);
                }

                ArgumentNullException.ThrowIfNull(user);
                ArgumentNullException.ThrowIfNull(user.Value);

                if (!user.EmailConfirm)
                {
                    var token = Memy.Shared.Helper.ConvertByteString.ConvertToString(user.Value);
                    throw new UnauthorizedAccessException(token);
                }

                var result = new UserStorage();
                LoginUser.UserSimpleParse(user, result);

                _tokenManager.NewToken(new Token()
                {
                    Value = user.Value,
                    ExpiryDate = user.ExpiryDate
                });

                if (string.IsNullOrWhiteSpace(result.Role))
                {
                    result.Role = null;
                }

                var jsonBytes = Memy.Shared.Helper.ConvertByteString.ConvertToString(result);

                return jsonBytes;
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
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
            public bool EmailConfirm { get; set; }
            public static void UserSimpleParse(LoginUser? loginUser, UserStorage? storage)
            {
                if (storage is not null && loginUser is not null)
                {
                    storage.UserName = loginUser.Nick;
                    storage.Token = loginUser.Value.ToString();
                    storage.Role = loginUser.Role;
                }
            }
        }
    }

}
