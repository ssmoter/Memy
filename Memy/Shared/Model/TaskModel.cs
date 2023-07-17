namespace Memy.Shared.Model
{
    public class FileModel
    {
        public string? ObjName { get; set; }
        public int ObjTyp { get; set; }
        public int ObjOrder { get; set; }
    }

    public class Tag
    {
        public string? Value { get; set; }
    }

    public class TaskModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool Banned { get; set; }
        public User? User { get; set; }
        public Tag[]? Tag { get; set; }
        public string? Description { get; set; }
        public FileModel[]? FileModel { get; set; }
        public ReactionModel? Reaction { get; set; }
        public ReportedModel? Reported { get; set; }
    }

    public class User
    {
        public string? Name { get; set; }
        public string? Avatar { get; set; }
    }
}
