using MongoDB.Bson;

namespace HAN.ADB.RDT.DataMigrationTool.Core
{
	public class User
	{
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string? AboutMe { get; set; }
        public int? Age { get; set; }
        public DateTime CreationDate { get; set; }
        public string DisplayName { get; set; }
        public int DownVotes { get; set; }
        public int UpVotes { get; set; }
        public string? EmailHash { get; set; }
        public DateTime LastAccessDate { get; set; }
        public string? Location { get; set; }
        public int Reputation { get; set; }
        public int Views { get; set; }
        public string? WebsiteUrl { get; set; }
        public int? AccountId { get; set; }

        // Related Data
        public IEnumerable<Badge>? Badges { get; set; }

        // Foreign references
        public IEnumerable<QuestionRef>? Questions { get; set; }
        public IEnumerable<AnswerRef>? Answers { get; set; }
        public IEnumerable<UserVote>? Votes { get; set; }
        public IEnumerable<UserComment>? Comments { get; set; }
    }

    public class Badge
    {
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AcuiredAt { get; set; }
    }

    public class UserComment
    {
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Body { get; set; }
        public int? Score { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class UserVote
    {
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public int PostId { get; set; }
        public int? BountyAmount { get; set; }
        public string VoteType { get; set; }
        public DateTime CreationDate { get; set; }
    }
}