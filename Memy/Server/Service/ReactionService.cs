using Memy.Server.Data;
using Memy.Server.Data.Reaction;
using Memy.Server.Helper;
using Memy.Shared.Model;

namespace Memy.Server.Service
{
    public class ReactionService
    {
        private readonly IReactionDataBase _reactionData;
        public ReactionService(IReactionDataBase reactionData)
        {
            _reactionData = reactionData;
        }

        public async Task<string> SetReaction(ReactionModel reaction, string token)
        {
            string procedure = "";
            switch (reaction.TypOfReaction)
            {
                case Shared.Helper.MyEnums.TypOfReaction.File:
                    {
                        procedure = ProcedureName.InsertReactionFile;
                    }
                    break;
                case Shared.Helper.MyEnums.TypOfReaction.Comment:
                    {
                        procedure = ProcedureName.InsertReactionComment;
                    }
                    break;
                case Shared.Helper.MyEnums.TypOfReaction.AnswerComment:
                    {
                        procedure = ProcedureName.InsertAnswerReactionComment;
                    }
                    break;
            }

            try
            {
                var json = await _reactionData.SetReaction(procedure, reaction.Id, reaction.Value, token);
                return json;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
