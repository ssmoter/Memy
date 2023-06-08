namespace Memy.Server.Data.Reaction
{
    public interface IReactionDataBase
    {
        Task<string> SetReaction(string procedure, int id, int value, string token);
    }
}