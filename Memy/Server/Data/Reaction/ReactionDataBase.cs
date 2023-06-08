using Memy.Server.Data.SqlDataAccess;

namespace Memy.Server.Data.Reaction
{
    public class ReactionDataBase : BaseData, IReactionDataBase
    {
        public ReactionDataBase(ISqlDataAccess sqlData) : base(sqlData)
        {
        }         

        public async Task<string> SetReaction(string procedure, int id, int value, string token)
        {
            sql.Clear();
            sql.Append("EXEC dbo.");
            sql.Append(procedure);
            sql.Append(" ");
            sql.Append(id);
            sql.Append(", ");
            sql.Append(value);
            sql.Append(", '");
            sql.Append(token);
            sql.Append("'");

            return await sqlData.LoadData<string>(sql.ToString());
        }

    }
}
