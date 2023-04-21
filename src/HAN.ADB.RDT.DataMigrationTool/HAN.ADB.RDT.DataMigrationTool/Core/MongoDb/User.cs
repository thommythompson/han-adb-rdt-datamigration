namespace HAN.ADB.RDT.DataMigrationTool.Core.MongoDb
{
	public class User
	{
        public int Id { get; set; }
        public string? AboutMe { get; set; }
        public int? Age { get; set; }
        public DateTime CreationDate { get; set; }
        public string DisplayName { get; set; }
        public int DownVotes { get; set; }
        public string? EmailHash { get; set; }
        public DateTime LastAccessDate { get; set; }
        public string? Location { get; set; }
        public int Reputation { get; set; }
        public int UpVotes { get; set; }
        public int Views { get; set; }
        public string? WebsiteUrl { get; set; }
        public int? AccountId { get; set; }

        // Related Data
        public IEnumerable<Badge>? Badges { get; set; }

        // Foreign references
        public IEnumerable<PostRef>? LastEditorForPosts { get; set; }
        public IEnumerable<PostRef>? Posts { get; set; }
        public IEnumerable<int>? Votes { get; set; }
        public IEnumerable<int>? Comments { get; set; }
    }

    public class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AcuiredAt { get; set; }
    }
}

