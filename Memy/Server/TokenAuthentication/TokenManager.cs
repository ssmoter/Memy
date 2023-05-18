using Memy.Server.Data.User;
using Memy.Shared.Model;

namespace Memy.Server.TokenAuthentication
{
    public interface ITokenManager
    {
        bool Authenticate(object token);
        void DeleteToken(string token);
        void NewToken(Token token);
        bool VerifyToken(Guid? token);
    }

    public class TokenManager : ITokenManager
    {
        private readonly IUserData IUser;
        private readonly List<Token?> TokenIds;

        public TokenManager(IUserData iUser)
        {
            IUser = iUser;
            TokenIds = new List<Token?>();
        }

        public bool Authenticate(object token)
        {
            if (token != null)
            {
                return true;
            }
            return false;
        }

        public void NewToken(Token token)
        {
            TokenIds.Add(token);
        }
        public void DeleteToken(string token)
        {
            for (int i = 0; i < TokenIds.Count; i++)
            {
                if (TokenIds[i].Value.ToString() == token)
                {
                    TokenIds.RemoveAt(i);
                    break;
                }
            }
        }
        public bool VerifyToken(Guid? token)
        {
            if (token == null)
            {
                return false;
            }
            //w pierwszej kolejności sprawdzana jest lista
            if (TokenIds.FirstOrDefault(
                x => x.Value == token
                && !x.DoNotLogOut
                | x.ExpiryDate > DateTimeOffset.Now) != null)
            {
                return true;
            }
            //w innym przypadku sprawdzana jest baza danych
            else if (IUser.CheckToken(token).Result)
            {
                return true;
            }
            return false;
        }

    }
}
