using Memy.Server.Data;
using Memy.Server.Data.User;
using Memy.Shared.Helper;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class RegisterService
    {
        private readonly IUserData _userData;
        public RegisterService(IUserData userData)
        {
            _userData = userData;
        }


        public async Task<string> RegisterUser(UserSimple userSimple)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrEmpty(userSimple.Email);
                if (!await _userData.EmailIsAvailable(userSimple.Email))
                {
                    throw new ArgumentException("email");
                }

                ArgumentNullException.ThrowIfNullOrEmpty(userSimple.Nick);
                if (!await _userData.NameIsAvailable(userSimple.Nick))
                {
                    throw new ArgumentException("name");
                }

                ArgumentNullException.ThrowIfNullOrEmpty(userSimple.Password);

                var result = await _userData.RegisterUser<Guid>(userSimple.Email, userSimple.Nick, userSimple.Password);

                return result.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string?> RegisterUserConfirm(string value)
        {
            try
            {
                LoginUser? user = null;

                user = await _userData.RegisterUserConfirm<LoginUser>(value);

                ArgumentNullException.ThrowIfNull(user);

                var result = new UserStorage();
                LoginUser.UserSimpleParse(user, result);
                if (result.Role == Roles.User)
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
