using Memy.Shared.Helper;

namespace Memy.Shared.Model
{
    public class ReactionModel
    {
        public ReactionModel(int id, int value, MyEnums.TypOfReaction typOfReaction)
        {
            Id = id;
            Value = value;
            TypOfReaction = typOfReaction;
        }
        public ReactionModel()
        {

        }
        public int Id { get; set; }
        public int ValueSum { get; set; }
        public int Value { get; set; }
        public MyEnums.TypOfReaction TypOfReaction { get; set; }

    }
}
