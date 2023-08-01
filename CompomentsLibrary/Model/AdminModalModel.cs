using CompomentsLibrary.Helper;

namespace CompomentsLibrary.Model
{
    public class AdminModalResponse
    {
        public string? Header { get; set; } = "";
        public string? Body { get; set; } = "";
        public PopupLevel.Level Level { get; set; } = PopupLevel.Level.None;
        public string Level2 { get; set; } = PopupLevel.Level.None.ToString();
        public string YesText { get; set; } = "Ok";
        public string NoText { get; set; } = "Cancel";
    }
    public class AdminModalResult
    {
        public string? Header { get; set; } = "";
        public string? Body { get; set; }="";
        public PopupLevel.Level Level { get; set; } = PopupLevel.Level.None;
    }
}
