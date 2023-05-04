using MongoDB.Bson;

namespace HAN.ADB.RDT.DataMigrationTool.Core
{
	public class QuestionRef
	{
        public ObjectId _id { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? ViewCount { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? Score { get; set; }
        public int? AnswerCount { get; set; }
        public bool? HasAcceptedAnswer { get; set; }
        public IEnumerable<string>? Tags { get; set; }
    }
}

