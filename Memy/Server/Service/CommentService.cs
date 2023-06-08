using Memy.Server.Data.Comment;
using Memy.Shared.Model;

using Newtonsoft.Json;

namespace Memy.Server.Service
{
    public class CommentService
    {
        private readonly ILogger _logger;
        private readonly ICommentData _commentData;

        public CommentService(ILogger logger, ICommentData commentData)
        {
            _logger = logger;
            _commentData = commentData;
        }

        public async Task<string> InsertComment(string procedure, string token, Comment commentModel, int orderTyp)
        {
            try
            {
                var json = JsonConvert.SerializeObject(commentModel);
                var result = await _commentData.InsertComment(procedure, token, json, orderTyp);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<string> GetComment(string procedure, int id, int orderTyp, string token)
        {
            try
            {
                return await _commentData.GetComment(procedure, id, orderTyp, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }



    }
}
