using HAN.ADB.RDT.DataMigrationTool.Core.Mssql;

namespace HAN.ADB.RDT.DataMigrationTool.Core.MongoDb
{
	public class Post
	{
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Body { get; set; }

        // MetaData
        public DateTime? ClosedDate { get; set; }
        public DateTime? CommunityOwnedDate { get; set; }
        public DateTime CreationDate { get; set; }
        public int? FavoriteCount { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime? LastEditDate { get; set; }
        public string PostType { get; set; }
        public int Score { get; set; }
        public string? Tags { get; set; }
        public int ViewCount { get; set; }

        // Related Data
        public IEnumerable<Comment>? Comments { get; set; }
        public IEnumerable<Vote>? Votes { get; set; }

        // References
        public UserRef? LastEditorUser { get; set; }
        public UserRef? OwnerUser { get; set; }
        public PostRef? Parent { get; set; }
        public IEnumerable<PostRef>? Answers { get; set; }
        public IEnumerable<PostRef>? LinkedPosts { get; set; }
        public IEnumerable<PostRef>? DuplicatePosts { get; set; }
        public PostRef? AcceptedAnswer { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int? Score { get; set; }
        public DateTime CreationDate { get; set; }

        // References
        public UserRef? User { get; set; }
    }

    public class Vote
    {
        public int Id { get; set; }
        public int? BountyAmount { get; set; }
        public string VoteType { get; set; }
        public DateTime CreationDate { get; set; }

        // References
        public UserRef? User { get; set; }
    }
}