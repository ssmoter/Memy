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
            AnswerComment=2,
        }
        public enum OrderTyp
        {
            DateDESC= 0,
            DateASC = 1,
            ReactionDESC=2, 
            ReactionASC=3,
        }
    }
}
