using System.ComponentModel.DataAnnotations;

namespace Memy.Shared.Model
{
    public class UserSimple
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? Nick { get; set; }
        public string? Password { get; set; }
        public Token Token { get; set; }
        public UserSimple()
        {
            Token = new Token();
        }
    }
}
