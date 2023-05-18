namespace Memy.Shared.Model
{
    public class Token
    {
        public Guid? Value { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        public bool DoNotLogOut { get; set; } = false;
        public Token(Guid value, DateTimeOffset expiryDate)
        {
            Value = value;
            ExpiryDate = expiryDate;
        }

        public Token()
        { }

    }
}
