using Memy.Server.Data.Comment;
using Memy.Shared.Model;

using Newtonsoft.Json;

namespace Memy.Server.Service
{
    internal class CommentService
    {
        private readonly ICommentData _commentData;

        public CommentService(ICommentData commentData)
        {
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
                    comment[i] = GetTask.ParseToComment(task[i]);
                }

                return comment;
            }
            catch (Exception)
            {
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
                    comment[i] = GetTask.ParseToComment(task[i]);
                }

                return comment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommentModel[]> GetLikeUserComment(string token, int orderTyp)
        {
            try
            {
                var task = await _commentData.GetLikeUserComment<GetTask>(orderTyp, token);

                var comment = new CommentModel[task.Length];
                for (int i = 0; i < comment.Length; i++)
                {
                    comment[i] = GetTask.ParseToComment(task[i]);
                }

                return comment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommentModel[]> GetUserComment(string? name, int orderTyp)
        {
            try
            {
                var task = await _commentData.GetUserComment<GetTask>(orderTyp, name);

                var comment = new CommentModel[task.Length];
                for (int i = 0; i < comment.Length; i++)
                {
                    comment[i] = GetTask.ParseToComment(task[i]);
                }

                return comment;
            }
            catch (Exception)
            {
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

            public static T? GetValue<T>(string? value)
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


            public static CommentModel ParseToComment(GetTask task)
            {
                var result = new CommentModel();
                result.Id = task.Id;
                if (!string.IsNullOrWhiteSpace(task.Description))
                {
                    result.Description = task.Description;
                }
                result.Date = task.Date;
                result.ObjectId = task.FileSimpleId;
                if (!string.IsNullOrWhiteSpace(task.User))
                {
                    var user = GetValue<User>(task.User);
                    if (user is not null)
                        result.User = user;
                }
                if (!string.IsNullOrWhiteSpace(task.Reaction))
                {
                    var reaction = GetValue<ReactionModel>(task.Reaction);
                    if (reaction is not null)
                        result.Reaction = reaction;
                }
                return result;
            }
        }


    }
}
