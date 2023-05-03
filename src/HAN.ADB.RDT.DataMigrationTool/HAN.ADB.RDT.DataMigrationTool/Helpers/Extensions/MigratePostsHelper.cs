using HAN.ADB.RDT.DataMigrationTool.Core.MongoDb;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MongoDb;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.MsSql;
using HAN.ADB.RDT.DataMigrationTool.DataAccess.SqlLite;
using Microsoft.EntityFrameworkCore;

namespace HAN.ADB.RDT.DataMigrationTool.Helpers.Extensions
{
    public class MigratePostsHelper
    {
        private readonly SqlContext _sqlContext;
        private readonly MongoDbRepository _mongoDbRepository;
        private readonly ProgressContext _progressContext;

        public MigratePostsHelper(SqlContext sqlContext, MongoDbRepository mongoDbRepository, ProgressContext progressContext)
        {
            _sqlContext = sqlContext;
            _mongoDbRepository = mongoDbRepository;
            _progressContext = progressContext;
        }

        public async Task MigratePosts(int startId)
        {
            var results = await _sqlContext.Posts
                .OrderBy(e => e.Id)
                .Take(100)
                .Where(e => e.Id > startId && e.ParentId == null)
                .Select(e => new Core.MongoDb.Post
                {
                    Id = e.Id,
                    Title = e.Title,
                    Body = e.Body,
                    ClosedDate = e.ClosedDate,
                    CommunityOwnedDate = e.CommunityOwnedDate,
                    FavoriteCount = e.FavoriteCount,
                    LastActivityDate = e.LastActivityDate,
                    LastEditDate = e.LastEditDate,
                    PostType = e.PostType.Type,
                    Score = e.Score,
                    Tags = null != e.Tags ? new TagList(e.Tags) : null,
                    ViewCount = e.ViewCount,
                    Comments = e.Comments.Select(e => new Core.MongoDb.Comment
                    {
                        Id = e.Id,
                        Body = e.Text,
                        Score = e.Score,
                        User = null != e.User ? new Core.MongoDb.UserRef { Id = e.User.Id, DisplayName = e.User.DisplayName } : null 
                    }),
                    Votes = e.Votes.Select(e => new Core.MongoDb.Vote
                    {
                        Id = e.Id,
                        VoteType = e.VoteType.Name,
                        BountyAmount = e.BountyAmount,
                        CreationDate = e.CreationDate,
                        User = null != e.User ? new Core.MongoDb.UserRef { Id = e.User.Id, DisplayName = e.User.DisplayName } : null
                    }),
                    LastEditorUser = null != e.LastEditorUser ? new Core.MongoDb.UserRef { Id = e.LastEditorUser.Id, DisplayName = e.LastEditorUser.DisplayName } : null,
                    OwnerUser = null != e.OwnerUser ? new Core.MongoDb.UserRef { Id = e.OwnerUser.Id, DisplayName = e.OwnerUser.DisplayName } : null,
                    Parent = null != e.Parent ? new Core.MongoDb.PostRef { Id = e.Parent.Id, Title = e.Parent.Title } : null,
                    AcceptedAnswer = e.AcceptedAnswer.Id,
                    Answers = e.Answers.Select(e => new Core.MongoDb.Answer
                    {
                        Id = e.Id,
                        Body = e.Body,
                        ClosedDate = e.ClosedDate,
                        CommunityOwnedDate = e.CommunityOwnedDate,
                        CreationDate = e.CreationDate,
                        FavoriteCount = e.FavoriteCount,
                        LastActivityDate = e.LastActivityDate,
                        LastEditDate = e.LastActivityDate,
                        PostType = e.PostType.Type,
                        Score = e.Score,
                        ViewCount = e.ViewCount,
                        Comments = e.Comments.Select(e => new Core.MongoDb.Comment
                        {
                            Id = e.Id,
                            Body = e.Text,
                            Score = e.Score,
                            User = null != e.User ? new Core.MongoDb.UserRef { Id = e.User.Id, DisplayName = e.User.DisplayName } : null
                        }),
                        Votes = e.Votes.Select(e => new Core.MongoDb.Vote
                        {
                            Id = e.Id,
                            VoteType = e.VoteType.Name,
                            BountyAmount = e.BountyAmount,
                            CreationDate = e.CreationDate,
                            User = null != e.User ? new Core.MongoDb.UserRef { Id = e.User.Id, DisplayName = e.User.DisplayName } : null
                        }),
                        LastEditorUser = null != e.LastEditorUser ? new Core.MongoDb.UserRef { Id = e.LastEditorUser.Id, DisplayName = e.LastEditorUser.DisplayName } : null,
                        OwnerUser = null != e.OwnerUser ? new Core.MongoDb.UserRef { Id = e.OwnerUser.Id, DisplayName = e.OwnerUser.DisplayName } : null,
                    }),
                    LinkedPosts = e.PostLinks.Where(e => e.LinkType.Type == "Linked").Select(e => new Core.MongoDb.PostRef
                    {
                        Id = e.RelatedPost.Id,
                        Title = e.RelatedPost.Title
                    }),
                    DuplicatePosts = e.PostLinks.Where(e => e.LinkType.Type == "Duplicate").Select(e => new Core.MongoDb.PostRef
                    {
                        Id = e.RelatedPost.Id,
                        Title = e.RelatedPost.Title
                    })
                }
                ).ToListAsync();

            Console.WriteLine($"Found {results.Count()} results.");

            await _mongoDbRepository.InsertPosts(results);

            await _progressContext.PostProgresses.AddAsync(new Core.SqlLite.PostProgress() { LastInsertedId = results.Max(e => e.Id) });
        }
    }
}