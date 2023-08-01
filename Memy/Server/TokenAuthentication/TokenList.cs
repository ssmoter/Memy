using Memy.Shared.Model;

namespace Memy.Server.TokenAuthentication
{
    public class TokenList
    {
        public readonly List<Token> TokenIds;

        public TokenList()
        {
            TokenIds = new List<Token>();
        }
    }
}
