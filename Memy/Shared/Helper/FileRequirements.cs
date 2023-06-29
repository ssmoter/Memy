namespace Memy.Shared.Helper
{
    public static class FileRequirements
    {
        public static int MaxNumberOfFiles { get => 6; }
        public static int MaxSizeOfFile { get => 5 * 1024 * 1024; }
        public static string[] FileTypAccess =
        {
            "png",  //0
            "jpg",  //1
            "gif",  //2
            "mp4",  //3
        };
        public static string PatchFolderName { get => "unsafe_uploads"; }
    }

    public static class Categories
    {
        public static string Main { get => "main"; }
        public static string Waiting { get => "waiting"; }

    }
}
