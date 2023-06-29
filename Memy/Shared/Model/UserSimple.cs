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

    public class UserPublicModel
    {
        public int? Id { get; set; }
        public string? Nick { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
        public int SumTaskLike { get; set; }
        public int SumTaskUnLike { get; set; }
        public int NumberOfTask { get; set; }
        public string? Avatar { get; set; }
    }

}
