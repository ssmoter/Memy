using Memy.Shared.Helper;

namespace Memy.Shared.Model
{
    public class Error
    {
        public MyEnums.TaskName Typ { get; set; }
        public string? Message { get; set; }
    }
}
