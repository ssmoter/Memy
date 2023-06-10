namespace Memy.Shared.Model
{
    public class CommentModel : Comment
    {
        public ReactionModel Reaction { get; set; }
        public User User { get; set; }

        public CommentModel()
        {
            Reaction = new ReactionModel();
            User = new User();
        }
    }

    public class Comment
    {
        public int Id { get; set; }
        public int FileSimpleId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
    }
}
