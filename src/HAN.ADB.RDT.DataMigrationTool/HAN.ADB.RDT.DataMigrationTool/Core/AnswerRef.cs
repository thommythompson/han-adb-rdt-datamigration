using MongoDB.Bson;

namespace HAN.ADB.RDT.DataMigrationTool.Core
{
	public class AnswerRef
	{
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool? IsAcceptedAnswer { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Score { get; set; }
        public IEnumerable<string>? Tags { get; set; }
    }
}

