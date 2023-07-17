using Memy.Server.Data.User;

namespace Memy.Server.TokenAuthentication
{
    public interface IAdminTokenManager
    {
        bool VerifyToken(Guid? token);
    }

    public class AdminTokenManager : IAdminTokenManager
    {
        private readonly ILoginData _User;

        public AdminTokenManager(ILoginData iUser)
        {
            _User = iUser;
        }

        public bool VerifyToken(Guid? token)
        {
            if (token == null)
            {
                return false;
            }

            if (_User.CheckAdminToken(token).Result)
            {
                return true;
            }
            return false;
        }

    }
}
