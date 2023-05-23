namespace Memy.Shared.Helper
{
    public static class FileRequirements
    {
        public static int MaxNumberOfFiles { get => 3; }
        public static int MaxSizeOfFile { get => 3 * 1024 * 1024; }
        public static string[] FileTypAccess =
        {
            "png",
            "jpg",
        };
        public static string PatchFolderName { get => "unsafe_uploads"; }
    }

    public static class Categories
    {
        public static string Main { get => "main"; }
        public static string Waiting { get => "waiting"; }

    }
}
