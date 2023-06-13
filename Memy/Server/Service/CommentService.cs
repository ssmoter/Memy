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

        public async Task<CommentModel[]> InsertComment(string procedure, string token, Comment commentModel, int orderTyp)
        {
            try
            {
                var json = JsonConvert.SerializeObject(commentModel);
                var task = await _commentData.InsertComment<GetTask>(procedure, token, json, orderTyp);
                var comment = new CommentModel[task.Length];
                for (int i = 0; i < comment.Length; i++)
                {
                    comment[i] = new CommentModel();
                    comment[i].Id = task[i].Id;
                    comment[i].Description = task[i].Description;
                    comment[i].Date = task[i].Date;
                    comment[i].ObjectId = task[i].FileSimpleId;
                    comment[i].User = GetTask.GetValue<User>(task[i].User);
                    comment[i].Reaction = GetTask.GetValue<ReactionModel>(task[i].Reaction);
                }

                return comment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<CommentModel[]> GetComment(string procedure, int id, int orderTyp, string token)
        {
            try
            {
                var task = await _commentData.GetComment<GetTask>(procedure, id, orderTyp, token);

                var comment = new CommentModel[task.Length];
                for (int i = 0; i < comment.Length; i++)
                {
                    comment[i] = new CommentModel();
                    comment[i].Id = task[i].Id;
                    comment[i].Description = task[i].Description;
                    comment[i].Date = task[i].Date;
                    comment[i].ObjectId = task[i].FileSimpleId;
                    comment[i].User = GetTask.GetValue<User>(task[i].User);
                    comment[i].Reaction = GetTask.GetValue<ReactionModel>(task[i].Reaction);
                }

                return comment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        private class GetTask
        {
            public int Id { get; set; }
            public int FileSimpleId { get; set; }
            public DateTimeOffset Date { get; set; }
            public string? Description { get; set; }
            public string? User { get; set; }
            public string? Reaction { get; set; }

            public static T? GetValue<T>(string value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
                }
                else
                {
                    return default(T?);
                }
            }
        }


    }
}
