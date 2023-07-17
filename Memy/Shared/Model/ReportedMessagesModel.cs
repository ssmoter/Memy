namespace Memy.Shared.Model
{
    public class ReportedMessagesModel
    {
        public int Id { get; set; }
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public string? Header { get; set; }
        public string? Body { get; set; }
        public int Level { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool BeenChecked { get; set; }
        public bool BeenDelete { get; set; }
        public int FileSimpleId { get; set; }
    }
}
