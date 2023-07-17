namespace Memy.Shared.Helper
{
    public static class MyEnums
    {
        public enum TaskName
        {
            Error = 0,
            Ok = 1,
        }
        public enum TypOfReaction
        {
            File = 0,
            Comment = 1,
            AnswerComment = 2,
        }
        public enum OrderTyp
        {
            DateDESC = 0,
            DateASC = 1,
            ReactionDESC = 2,
            ReactionASC = 3,
        }
        public enum FileTyp
        {
            text = 1,
            image,
            video,
            YouTube,
        }
        public enum AvatarSize
        {
            AnswerComment = 1,
            Comment,
            File,
            Profile
        }
        public enum UpdateProfile
        {
            Password=0,
            Name,
            Avatar,
            Email,
        }

        public enum AdminDeleteBanType
        {
            AnswerComment = 1,
            Comment,
            File,
            Profile
        }

    }
}
