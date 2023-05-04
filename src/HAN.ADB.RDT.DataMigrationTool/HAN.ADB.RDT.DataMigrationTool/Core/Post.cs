using System.Collections;
using MongoDB.Bson;

namespace HAN.ADB.RDT.DataMigrationTool.Core
{
    public class Post
    {
        public ObjectId _id { get; set; }
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
        public IEnumerable<string>? Tags { get; set; }
        public int ViewCount { get; set; }

        // Related Data
        public IEnumerable<PostComment>? Comments { get; set; }
        public IEnumerable<PostVote>? Votes { get; set; }

        // References
        public UserRef? LastEditorUser { get; set; }
        public UserRef? OwnerUser { get; set; }
        public IEnumerable<Answer>? Answers { get; set; }
        public IEnumerable<PostRef>? RelatedPosts { get; set; }
        public IEnumerable<PostRef>? DuplicatePosts { get; set; }
        public int? AcceptedAnswerId { get; set; }
    }

    public class Answer
    {
        public ObjectId _id { get; set; }
        public int Id { get; set; }
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
        public int ViewCount { get; set; }

        // Related Data
        public IEnumerable<PostComment>? Comments { get; set; }
        public IEnumerable<PostVote>? Votes { get; set; }

        // References
        public UserRef? LastEditorUser { get; set; }
        public UserRef? OwnerUser { get; set; }
    }

    public class PostComment
    {
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string Body { get; set; }
        public int? Score { get; set; }
        public DateTime CreationDate { get; set; }

        // References
        public UserRef? User { get; set; }
    }

    public class PostVote
    {
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public int? BountyAmount { get; set; }
        public string VoteType { get; set; }
        public DateTime CreationDate { get; set; }

        // References
        public UserRef? User { get; set; }
    }
}