namespace Memy.Shared.Helper
{
    public static class Routes
    {
        //Controller
        public static string UserLog { get => "UserLog"; }
        public static string User { get => "User"; }
        public static string File { get => "File"; }
        public static string Comment { get => "Comment"; }
        public static string Reaction { get => "Reaction"; }
        public static string Reported { get => "Reported"; }
        public static string Tag { get => "Tag"; }
        public static string ReportedMessages { get => "ReportedMessages"; }

        //AdminController
        public static string FileAdmin { get => "FileAdmin"; }
        public static string CommentAdmin { get => "CommentAdmin"; }

        public static string Delete { get => "Delete"; }
        public static string Ban { get => "Ban"; }


        //Routes
        public static string Img { get => "img"; }
        public static string Video { get => "video"; }
        public static string Register { get => "register"; }
        public static string Email { get => "email"; }
    }
}