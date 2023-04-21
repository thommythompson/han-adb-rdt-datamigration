using System;
namespace HAN.ADB.RDT.DataMigrationTool.Core.Mssql
{
	public class Post
	{
		public int Id { get; set; }
		public int? AcceptedAnswerId { get; set; }
		public Post? AcceptedAnswer { get; set; }
		public int? AnswerCount { get; set; }
		public string Body { get; set; }
		public DateTime? ClosedDate { get; set; }
		public int? CommentCount { get; set; }
		public DateTime? CommunityOwnedDate { get; set; }
		public DateTime CreationDate { get; set; }
		public int? FavoriteCount { get; set; }
		public DateTime LastActivityDate { get; set; }
		public DateTime? LastEditDate { get; set; }
		public string? LastEditorDisplayName { get; set; }
		public int? LastEditorUserId { get; set; }
		public User? LastEditorUser { get; set; }
		public int? OwnerUserId { get; set; }
		public User? OwnerUser { get; set; }
		public int? ParentId { get; set; }
		public Post? Parent { get; set; }
		public int PostTypeId { get; set; }
		public PostType PostType { get; set; }
		public int Score { get; set; }
		public string? Tags { get; set; }
		public string? Title { get; set; }
		public int ViewCount { get; set; }

        // Foreign references
		public ICollection<Comment>? Comments { get; set; }
		public Post? AcceptedAnswerFor { get; set; }
		public ICollection<Post>? Answers { get; set; }
		public ICollection<Vote>? Votes { get; set; }
		public ICollection<PostLink>? PostLinks { get; set; }
        public ICollection<PostLink>? RelatedPostFor { get; set; }
    }
}